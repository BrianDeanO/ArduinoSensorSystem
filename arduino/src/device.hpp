#pragma once

#include "../config.hpp"
#include "sensor.hpp"
#include "client.hpp"

class Device {
public:
	Device(const char* id, Sensor* sensors, int num_sensors, DataClient* client) {
		this->_id = id;
		this->sensors = sensors;
		this->num_sensors = num_sensors;
		this->client = client;
	}

	// Acquire new data and send all cached data to the server. This may send multiple packets
	// to the server if the data does not fit in a single buffer.
	virtual void update();

	void set_record_interval(unsigned interval) {
		record_interval = interval;
	}

	// The timestamp of the next update
	uint64_t next_update() {
		return last_update + record_interval;
	}

	// Instruct sensors to acquire and cache a new data point.
	void acquire_data();

	// Register this device with the server. This should be called once on startup.
	virtual bool register_device();

	void get_register_command(SizedBuf& buf);
	bool get_data_command(SizedBuf& buf);

private:
	const char* _id;
	Sensor* sensors;
	uint8_t num_sensors; // MAX: 254 (last_sensor_read must be able to be 1 greater)

	// When reading data points into the send buffer, we might run out of room.
	// This field keeps track of the last sensor we were on, so we can continue
	// if this is <= num_sensors.
	uint8_t _last_read_sensor = 0;


	unsigned record_interval = DEFAULT_RECORD_INTERVAL;
	uint64_t last_update;
	DataClient* client;
};