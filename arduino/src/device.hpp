#pragma once

#include <bits/types/time_t.h>

#include "../config.hpp"
#include "sensor.hpp"
#include "http_client.hpp"
#include "time_client.hpp"

class Device {
public:
	Device(const char* id, Sensor* sensors, int num_sensors, HttpClient* client, TimeClient* time_client) {
		this->id = id;
		this->sensors = sensors;
		this->num_sensors = num_sensors;
		this->http = client;
		this->time = time_client;
	}

	virtual void update();

	void set_record_interval(unsigned interval) {
		record_interval = interval;
	}

	time_t next_update() {
		return last_update + record_interval;
	}

	virtual void register_device();
	virtual void get_data();
	virtual bool send_data();

private:
	const char* id;
	Sensor* sensors;
	unsigned num_sensors;

	unsigned record_interval = DEFAULT_RECORD_INTERVAL;
	time_t last_update;

	HttpClient* http;
	TimeClient* time;
};