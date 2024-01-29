#pragma once

#include "../config.hpp"
#include "sensor.hpp"
#include "client.hpp"

class Device {
public:
	Device(Sensor* sensors, int num_sensors, DataClient* client) {
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

private:
	const char* _id;
	Sensor* sensors;
	uint8_t num_sensors; // MAX: 254

	uint32_t id; // The id we get from the database (our primary key)
	unsigned record_interval = DEFAULT_RECORD_INTERVAL;
	uint64_t last_update;
	DataClient* client;
};