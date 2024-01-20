#include "device.hpp"
#include "../config.hpp"
#include "util.hpp"

bool Device::register_device() {
	char buffer[SEND_BUFFER_SIZE];
	SizedBuf buf(buffer, SEND_BUFFER_SIZE);
	get_register_command(buf);

	DEBUG(Serial.println("Sending register command"));
	return client->send(buf.buffer, buf.len);
}

void Device::update() {
	uint64_t current_time = client->get_time();

	if(current_time >= next_update()) {
		DEBUG(Serial.println("Starting update"));
		acquire_data(); // Read data from sensors into cache

		char buffer[SEND_BUFFER_SIZE];
		SizedBuf buf(buffer, SEND_BUFFER_SIZE);

		bool done = false;
		while(!done) {
			buf.clear();
			done = get_data_command(buf);

			DEBUG(Serial.println("Command len: " + String(buf.len)));
			if(client->send(buf.buffer, buf.len)) {
				// Data successfully sent, reset sensors that had all
				// their data sent
				for(unsigned i = 0; i <= _last_read_sensor; i++) {
					sensors[i].reset();
				}
			}
			else {
				// Send failed, try again in a minute
				for(unsigned i = 0; i <= num_sensors; i++) {
					// Tell all sensors with data to resend all their
					// data again next time we read from them.
					sensors[i].reset_last_sent();
				}

				Serial.println("Failed to send data, retrying in 60 seconds");
				delay(60000);
			}
		}

		last_update = current_time;
	}
}

void Device::acquire_data() {
	for(unsigned i = 0; i < num_sensors; i++) {
		for(unsigned j = 0; j < 10; j++) {
			sensors[i].acquire_data_point(client->get_time());
		}
	}
}

void Device::get_register_command(SizedBuf& buf) {
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
}

bool Device::get_data_command(SizedBuf& buf) {
	uint8_t command = 2;
	buf.append((void*)&command, sizeof(uint8_t));
	buf.append((void*)this->_id, 32);
	buf.append((void*)this->num_sensors, sizeof(this->num_sensors));

	for(unsigned i = 0; i < num_sensors; i++) {
		DEBUG(Serial.println("Getting points from sensor " + String(i)));
		if(!sensors[i].copy_points(buf)) {
			DEBUG(Serial.println("Sensor " + String(i) + " copy_points returned false. buf.len = " + String(buf.len)));
			return false;
		}
		_last_read_sensor = i;
	}

	return true;
}