#include "device.hpp"
#include "../config.hpp"

#define DEFAULT_DEVICE_JSON  R"({"deviceIdent":")" DEVICE_IDENT R"(","deviceName":")" DEFAULT_DEVICE_NAME R"(","deviceType":")" DEFAULT_DEVICE_TYPE R"("})"

void Device::init() {
	for(unsigned i = 0; i < num_sensors; i++) {
		sensors[i]->init();
	}

    while(!register_device()) { 
        DEBUG("Failed to register device, retrying in 3 seconds\n");
        delay(3000); 
	}
}

bool Device::register_device() {
	DEBUG("Sending register command\n");
	char buf[RESPONSE_BUFFER_SIZE];

	int result = client->get("/api/Device/ident/" DEVICE_IDENT, buf, RESPONSE_BUFFER_SIZE);
	if(result > 0) {
		JsonDocument j;
		deserializeJson(j, buf);
		if(!j.containsKey("deviceID")) {
			DEBUG("ERR: Registration response contained no id!\n");
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
		result = client->post("/api/Device", DEFAULT_DEVICE_JSON, buf, RESPONSE_BUFFER_SIZE);
		if(result > 0) {
			JsonDocument j;
			deserializeJson(j, buf);
			if(!j.containsKey("deviceID")) {
				DEBUG("ERR: New Registration response contained no id!\n");
				return false;
			}

			this->_id = j["deviceID"].as<uint32_t>();
			DEBUG("Got new device db ID: %d\n", this->_id);
			// TODO: Set poll time
		}
		else {
			DEBUG("ERR: sending device register command: %d\n", result);
			return false;
		}
	}
	else {
		DEBUG("ERR: sending device GET command: %d\n", result);
		return false;
	}

	for(unsigned i = 0; i < num_sensors; i++) {
		if(!sensors[i]->register_sensor(this->client, buf, RESPONSE_BUFFER_SIZE, _id))
			return false;
	}

	return true;
}

void Device::update() {
	uint64_t current_time = client->get_time();

	if(current_time >= next_update()) {
		DEBUG("Starting update -----\n");
		acquire_data(); // Read data from sensors into cache

		JsonDocument j;
		char buf[RESPONSE_BUFFER_SIZE];
		for(unsigned i = 0; i < num_sensors; i++) {
			Sensor* sensor = sensors[i];

			Datapoint point;
			while(sensor->get_last_point(point)) {
				j.clear();
				j[F("sensorID")] = sensor->id();
				j[F("channelID")] = point.channel;
				j[F("dataValue")] = point.value;
				j[F("timeRecorded")] = point.time;
				j[F("dataUnit")] = sensor->channel_units(point.channel);
				serializeJson(j, buf);

				int retries = 0;
				int result = 0;
				const int max_retries = 3;
				while(retries < max_retries) {
					result = client->post("/api/SensorData", buf, nullptr, 0);
					// If an error occurs, retry. If the error is -422, the data point already exists so throw it away
					if(result < 0 && result != -422) {
						retries++;
						delay(5000);
					}
					else {
						sensor->pop_last_point(); // Point sucessfully sent, remove it from the cache
						break;
					}
				}
				if (retries == max_retries) {
					DEBUG("Failed to send data point after %d retries: got code %d\n", retries, result);
					break;
				}
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
