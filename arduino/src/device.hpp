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

	virtual void update();

	void set_record_interval(unsigned interval) {
		record_interval = interval;
	}

	// The timestamp of the next update
	uint64_t next_update() {
		return last_update + record_interval;
	}

	// Register this device with the server. This should be called once on startup.
	virtual bool register_device();

	// Instruct sensors to acquire and cache a new data point.
	virtual void get_data();

	// Send all cached data to the server. This may send multiple packets
	// to the server if the data does not fit in a single buffer.
	virtual bool send_data();

private:
	const char* _id;
	Sensor* sensors;
	uint16_t num_sensors;

	unsigned record_interval = DEFAULT_RECORD_INTERVAL;
	uint64_t last_update;
	DataClient* client;
};