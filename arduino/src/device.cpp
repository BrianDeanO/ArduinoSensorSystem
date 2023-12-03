#include "device.hpp"
#include "../config.hpp"

void Device::register_device() {
	char buffer[SEND_BUFFER_SIZE];
	for(unsigned i = 0; i < num_sensors; i++) {
		// TODO: build json string with each sensor's id/units, or have the sensor do it
	}
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
	for(unsigned i = 0; i < num_sensors; i++) {
		// TODO
	}
}