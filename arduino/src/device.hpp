#pragma once

#include "../config.hpp"
#include "sensor.hpp"
#include "dataClient.hpp"

class Device {
public:
	Device(Sensor** sensors, int num_sensors, DataClient* client) {
		this->sensors = sensors;
		this->num_sensors = num_sensors;
		this->client = client;
	}

	virtual void init();

	// Acquire new data and send all cached data to the server. This may send multiple packets
	// to the server if the data does not fit in a single buffer.
	virtual void update(uint64_t current_time);

	virtual void get_config();

	void set_update_interval(unsigned interval) {
		update_interval = interval;
	}

	// The timestamp of the next update
	uint64_t next_update() {
		return _last_update + update_interval;
	}

	uint64_t last_update() {
		return _last_update;
	}

	// Instruct sensors to acquire and cache a new data point.
	void acquire_data();

	// Register this device with the server. This should be called once on startup.
	virtual bool register_device();

private:
	Sensor** sensors;
	uint8_t num_sensors = 0; // MAX: 254

	uint32_t _id; // The id we get from the database (our primary key)
	unsigned update_interval = 30; // Default to 30 in case the server doesn't respond with a config interval
	uint64_t _last_update = 0;
	DataClient* client;
};