#pragma once

#include "../common.hpp"
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

	virtual bool get_config();
	bool poke_device();

	// The timestamp of the next update
	uint64_t next_update() {
		return _last_update + _update_interval;
	}

	uint64_t last_update() {
		return _last_update;
	}

	void set_update_interval(unsigned interval) {
		if(interval < MIN_UPDATE_INTERVAL) {
			DEBUG("ERR: getConfig interval too short, using minimum %d seconds\n", MIN_UPDATE_INTERVAL);
			_update_interval = MIN_UPDATE_INTERVAL;
			return;
		}
		_update_interval = interval;
	}

	// Instruct sensors to acquire and cache a new data point.
	void acquire_data();

	// Register this device with the server. This should be called once on startup.
	virtual bool register_device();

	uint32_t id() {
		return _id;
	}

	unsigned update_interval() {
		return _update_interval;
	}

private:
	Sensor** sensors;
	uint8_t num_sensors = 0; // MAX: 254

	uint32_t _id; // The id we get from the database (our primary key)
	unsigned _update_interval = MIN_UPDATE_INTERVAL;
	uint64_t _last_update = 0;
	DataClient* client;
};