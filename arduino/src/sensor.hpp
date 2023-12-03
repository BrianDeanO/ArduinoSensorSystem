#pragma once

#include <bits/types/time_t.h>
#include <cstdio>
#include <cstring>

#include "../config.hpp"
#include "driver.hpp"

struct Datapoint {
	double value;
	time_t time;
};

class Sensor {
public:
	Sensor(const char* id, SensorDriver* driver) {
		this->_id = id;
		this->_driver = driver;
	}

	// Acquire a new data point from the sensor, storing it in this sensor's cache.
	// Use pop_data_point() to retrieve the data point.
	virtual void acquire_data_point(time_t time) {
		Datapoint& point = _cache[_cache_index++];
		point.value = _driver->acquire_data_point();
		point.time = time;
	}

	virtual void serialize(char* buffer) {
		sprintf(buffer, "{\"id\":\"%s\",\"data\":[", _id);
		buffer += strlen(buffer) - 1;

		if(_cache_index == 0) {
			sprintf(buffer, "]}");
			return;
		}

		while(_cache_index > 0) {
			Datapoint& point = _cache[--_cache_index];
			sprintf(buffer, "{\"v\":%lf,\"t\":%ld},", point.value, point.time);
			buffer += strlen(buffer) - 1;
		}

		sprintf(buffer - 1, "]}"); // -1 to cut off trailing comma
	}

	void reset() {
		_cache_index = 0;
	}

	const char* id() {
		return _id;
	}

private:
	unsigned _cache_index = 0;
	Datapoint _cache[DATAPOINT_CACHE_SIZE];
	const char* _id;
	SensorDriver* _driver;	
};