#pragma once

#include <bits/types/time_t.h>
#include <cstdio>
#include <cstring>

#include "../config.hpp"
#include "driver.hpp"
#include "util.hpp"

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

	virtual bool serialize_info(CString& str) {
		TRY_APPEND(str, "{\"id\":\"");
		TRY_APPEND(str, _id);
		TRY_APPEND(str, "\",\"units\":\"");
		TRY_APPEND(str, _driver->units());
		TRY_APPEND(str, "\"},");
		return true;
	}

	// Serialize this sensor's info into the given buffer.
	virtual bool serialize_data(CString& str) {
		// TODO: I am starting to think that json is going to be too inefficient
		// for the limited memory we have, consider using a binary stream and just
		// sending the raw bytes over the wire.
		TRY_APPEND(str, "{\"id\":\"");
		TRY_APPEND(str, _id);
		TRY_APPEND(str, "\",\"data\":[");

		if(_cache_index == 0) {
			TRY_APPEND(str, "]}");
			return true;
		}

		while(_cache_index > 0) {
			if(str.len() + 50 > str.capacity()) {
				break;
			}

			Datapoint& point = _cache[--_cache_index];
			char buffer[16];

			TRY_APPEND(str, "{\"v\":");
			sprintf(buffer, "%.13lf", point.value);
			if(!str.append(buffer)) {
				break;
			}

			TRY_APPEND(str, ",\"t\":");
			sprintf(buffer, "%ld", point.time);
			if(!str.append(buffer)) {
				break;
			}
			TRY_APPEND(str, "},");
		}

		TRY_APPEND(str, "]}", -1); // -1 to cut off trailing comma
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