#include "device.hpp"
#include "../config.hpp"
#include "util.hpp"

#define DEFAULT_DEVICE_JSON  R"({"deviceIdent":")" DEVICE_IDENT R"(","deviceName":")" DEFAULT_DEVICE_NAME R"(","deviceType":")" DEFAULT_DEVICE_TYPE R"("})"

bool Device::register_device() {
	JsonDocument j;

	DEBUG("Sending register command");
	char buf[RESPONSE_BUFFER_SIZE];
	int result = client.get("/api/Devices/ident/" DEVICE_IDENTIFIER, buf, RESPONSE_BUFFER_SIZE);

	if(result > 0) {
		deserializeJson(j, buf);
		if(!j.containsKey("deviceID")) {
			DEBUG("Registration response contained no id!");
			return false;
		}

		this->id = j["deviceID"].as<uint32_t>();
		// TODO: Set poll time
	}
	// Check for 400 level response codes, indicating the identifier does not 
	// yet exist
	else if(result / -100 == 4) {
		// Register a new device with our defaults
		result = client.post("/api/Devices", DEFAULT_DEVICE_JSON, buf, RESPONSE_BUFFER_SIZE);
		if(result > 0) {
			if(!j.containsKey("deviceID")) {
				DEBUG("New Registration response contained no id!");
				return false;
			}

			this->id = j["deviceID"].as<uint32_t>();
			// TODO: Set poll time
		}
		else {
			return false;
		}
	}
	else {
		return false;
	}

	// TODO: Register sensors and device/sensor associations

	return true;
}

void Device::update() {
	uint64_t current_time = client->get_time();

	if(current_time >= next_update()) {
		DEBUG(Serial.println("Starting update"));
		acquire_data(); // Read data from sensors into cache

		bool done = false;
		while(!done) {
			// TODO: Redo sending points again 
		}

		last_update = current_time;
	}
}

void Device::acquire_data() {
	for(unsigned i = 0; i < num_sensors; i++) {
		// TODO: What was this for? Reliability?
		// for(unsigned j = 0; j < 10; j++) {
			sensors[i].acquire_data_point(client->get_time());
		// }
	}
}
