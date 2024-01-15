#include "device.hpp"
#include "../config.hpp"
#include "util.hpp"

bool Device::register_device() {
	char buffer[SEND_BUFFER_SIZE];
	SizedBuf buf(buffer, SEND_BUFFER_SIZE);

	uint8_t command = 1;
	buf.append((void*)&command, sizeof(uint8_t));
	buf.append((void*)this->_id, 32);
	buf.append((void*)this->num_sensors, sizeof(this->num_sensors));

	for(unsigned i = 0; i < num_sensors; i++) {
		if(!sensors[i].copy_sensor_info(buf)) {
			// TODO: Error handling on too many sensors for buffer
			break;
		}
	}

	return client->send(buffer);
}

void Device::update() {
	uint64_t current_time = client->get_time();

	if(current_time >= next_update()) {
		get_data();
		if(send_data()) {
			// Data successfully sent, reset sensors
			for(unsigned i = 0; i < num_sensors; i++) {
				sensors[i].reset();
			}
			last_update = current_time;
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
		sensor.acquire_data_point(client->get_time());
	}
}

bool Device::send_data() {
	char buffer[SEND_BUFFER_SIZE];
	SizedBuf buf(buffer, SEND_BUFFER_SIZE);

	uint8_t command = 2;
	buf.append((void*)&command, sizeof(uint8_t));
	buf.append((void*)this->_id, 32);
	buf.append((void*)this->num_sensors, sizeof(this->num_sensors));

	for(unsigned i = 0; i < num_sensors; i++) {
		if(!sensors[i].copy_points(buf)) {
			// TODO: Error handling
			break;
		}
	}

	return client->send(buffer);
}