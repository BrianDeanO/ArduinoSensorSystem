#include "device.hpp"
#include "../config.hpp"
#include "util.hpp"

bool Device::register_device() {
	char buffer[SEND_BUFFER_SIZE];
	CString str(buffer, SEND_BUFFER_SIZE);

	str.append(R"({"cmd":"r","id":")");
	str.append(id);
	str.append(R"(","sensors":[)");
	for(unsigned i = 0; i < num_sensors; i++) {
		// TODO: build json string with each sensor's id/units, or have the sensor do it
		if(!sensors[i].serialize_info(str)) {
			// TODO: Error handling
			break;
		}
	}
	str.append("]}");

	int result = http->post("/api/device/register", buffer, "application/json", NULL, 0);
	return result == 200;
}

void Device::update() {
	time->update(); // Periodically sync time with the NTP server

	if(time->get_time() >= next_update()) {
		get_data();
		if(send_data()) {
			// Data successfully sent, reset sensors
			for(unsigned i = 0; i < num_sensors; i++) {
				sensors[i].reset();
			}
			last_update = time->get_time();
		}
		else {
			// Send failed, try again in a minute
			last_update += 60;
		}
	}
}

void Device::get_data() {
	for(unsigned i = 0; i < num_sensors; i++) {
		Sensor& sensor = sensors[i];
		sensor.acquire_data_point(time->get_time());
	}
}

bool Device::send_data() {
	char buffer[SEND_BUFFER_SIZE];
	CString str(buffer, SEND_BUFFER_SIZE);

	str.append(R"({"cmd":"d","id":")");
	str.append(id);
	str.append(R"(","data":[)");
	for(unsigned i = 0; i < num_sensors; i++) {
		if(!sensors[i].serialize_data(str)) {
			// TODO: Error handling
			break;
		}
	}
	str.append("]}");

	int result = http->post("/api/device/data", buffer, "application/json", NULL, 0);
	return result == 200;
}