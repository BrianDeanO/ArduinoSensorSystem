#pragma once

#include "../config.hpp"
#include "driver.hpp"
#include "util.hpp"

struct Datapoint {
	double value;
	uint64_t time;
};

class Sensor {
public:
	Sensor(const char* id, SensorDriver* driver) {
		this->_id = id;
		this->_driver = driver;
	}

	// Acquire a new data point from the sensor, storing it in this sensor's cache.
	// Use pop_data_point() to retrieve the data point.
	virtual void acquire_data_point(uint64_t time) {
		Datapoint& point = _cache[_cache_index++];
		point.value = _driver->acquire_data_point();
		point.time = time;
	}

	// Returns true if all points were successfully added, false if there
	// wasn't enough room for all points.
	// 
	// Layout:
	//    char[32]: id
	//    char[32]: units
	virtual bool copy_sensor_info(SizedBuf& buf) {
		if(!buf.has_capacity(64))
			return false;

		buf.append((void*)_id, 32);
		buf.append((void*)_driver->units(), 32);

		return true;
	}

	// Serialize this sensor's info into the given buffer.
	// Returns true if all points were successfully added, false if there
	// wasn't enough room for all points.
	//
	// Layout:
	//    Header
	//        char[32]: id
	//        unsigned: num_points
	//    Points[num_points]
	//        double: value
	//        unsigned long long: time_recorded
	virtual bool copy_points(SizedBuf& buf) {
		// Copy the sensor header:
		//    char[32]: id
		//    unsigned: num_points
		if(!buf.has_capacity(32 + sizeof(uint32_t))) {
			return false;
		}
		buf.append((void*)this->_id, 32);

		uint32_t reportable_points = buf.remaining() / sizeof(Datapoint);
		if(reportable_points > _cache_index) {
			reportable_points = _cache_index;
		}
		buf.append((void*)&reportable_points, sizeof(uint32_t));

		if(reportable_points == 0) {
			// No points to report (either because there are 0 points or we have
			// no room). Return false if there are still points left
			return _cache_index == 0;
		}

		while(reportable_points > 0) {
			if(!buf.has_capacity(sizeof(Datapoint))) {
				// This should be covered by the above reportable_points
				// check, but just in case break here
				break;
			}

			Datapoint& point = _cache[_cache_index];
			_cache_index -= 1;
			reportable_points -= 1;
			buf.append((void*)&point, sizeof(Datapoint));
		}

		return true;
	}

	void reset() {
		_cache_index = 0;
	}

	const char* id() {
		return _id;
	}

private:
	uint32_t _cache_index = 0;
	Datapoint _cache[DATAPOINT_CACHE_SIZE];
	const char* _id;
	SensorDriver* _driver;	
};