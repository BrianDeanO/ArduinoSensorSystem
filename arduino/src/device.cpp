#include "device.hpp"
#include "../config.hpp"

#define DEFAULT_DEVICE_JSON  R"({"deviceIdent":")" DEVICE_IDENT R"(","deviceName":")" DEFAULT_DEVICE_NAME R"(","deviceType":")" DEFAULT_DEVICE_TYPE R"("})"

bool Device::register_device() {
	DEBUG("Sending register command\n");
	char buf[RESPONSE_BUFFER_SIZE];

	int result = client->get("/api/Devices/ident/" DEVICE_IDENT, buf, RESPONSE_BUFFER_SIZE);
	if(result > 0) {
		JsonDocument j;
		deserializeJson(j, buf);
		if(!j.containsKey("deviceID")) {
			DEBUG("Registration response contained no id!\n");
			return false;
		}

		this->_id = j["deviceID"].as<uint32_t>();
		DEBUG("Got device db ID: %d\n", this->_id);
		// TODO: Set poll time
	}
	// Check for 400 level response codes, indicating the identifier does not 
	// yet exist
	else if(result / -100 == 4) {
		// Register a new device with our defaults
		result = client->post("/api/Devices", DEFAULT_DEVICE_JSON, buf, RESPONSE_BUFFER_SIZE);
		if(result > 0) {
			JsonDocument j;
			deserializeJson(j, buf);
			if(!j.containsKey("deviceID")) {
				DEBUG("New Registration response contained no id!\n");
				return false;
			}

			this->_id = j["deviceID"].as<uint32_t>();
			DEBUG("Got new device db ID: %d\n", this->_id);
			// TODO: Set poll time
		}
		else {
			DEBUG("Error sending device register command: %d\n", result);
			return false;
		}
	}
	else {
		DEBUG("Error sending device get command: %d\n", result);
		return false;
	}

	for(unsigned i = 0; i < num_sensors; i++) {
		if(!sensors[i]->register_sensor(this->client, buf, RESPONSE_BUFFER_SIZE))
			return false;

		JsonDocument j;
		j["deviceID"] = this->_id;
		j["sensorID"] = sensors[i]->id();
		serializeJson(j, buf, RESPONSE_BUFFER_SIZE);

		result = client->post("/api/DeviceSensors", buf, nullptr, 0);
		if(result < 0)
			return false;
	}

	return true;
}

void Device::update() {
	uint64_t current_time = client->get_time();

	if(current_time >= next_update()) {
		DEBUG("Starting update\n");
		acquire_data(); // Read data from sensors into cache

		JsonDocument j;
		char buf[RESPONSE_BUFFER_SIZE];
		for(unsigned i = 0; i < num_sensors; i++) {
			Sensor* sensor = sensors[i];

			Datapoint point;
			while(sensor->get_last_point(point)) {
				j.clear();
				j["sensorID"] = sensor->id();
				j["channelID"] = point.channel;
				j["dataValue"] = point.value;
				j["timeRecorded"] = point.time;
				j["dataUnit"] = sensor->channel_units(point.channel);
				serializeJson(j, buf);

				int retries = 0;
				while(retries < 2) {
					int result = client->post("/api/SensorDatas", buf, nullptr, 0);
					if(result < 0) {
						retries++;
					}
					else {
						break;
					}
				}
				sensor->pop_last_point(); // Point sucessfully sent, remove it from the cache
			}
		}

		last_update = current_time;
	}
}

void Device::acquire_data() {
	for(unsigned i = 0; i < num_sensors; i++) {
		sensors[i]->cache_data_point(client->get_time());
	}
}
