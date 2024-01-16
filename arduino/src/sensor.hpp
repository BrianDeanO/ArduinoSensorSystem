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
		if(_cache_index == 0) {
			// No points to report
			return true;
		}

		// Copy the sensor header:
		//    char[32]: id
		//    unsigned: num_points
		if(!buf.has_capacity(32 + sizeof(uint32_t))) {
			return false;
		}
		buf.append((void*)this->_id, 32);

		// First send since reset
		if(_last_sent_point == 0) {
			// We set up this _last_sent_point variable so we know to resend 
			// the points between last_sent_point..cache_index if the send over
			// the network fails. If a network send ever fails, the device can
			// just set _last_sent_point.
			//
			// Note: After this whole device is successfully sent, its reset()
			// should be called or else it'll keep resending all its data.
			_last_sent_point = _cache_index;
		}

		// Calculate how many points we can fit in the remaining space of buf
		uint32_t reportable_points = buf.remaining() / sizeof(Datapoint);
		if(reportable_points > _last_sent_point) {
			// Don't report more points than we have.
			reportable_points = _last_sent_point - 1;
			// -1 as both cache_index and last_sent_point point to the index
			// after the most recent data value.
		}
		// Append the number of points we'll be writing
		buf.append((void*)&reportable_points, sizeof(uint32_t));

		if(reportable_points == 0) {
			// No room to report any points. We know there are points left
			// from the above check, so return false
			return false;
		}

		while(reportable_points > 0) {
			if(!buf.has_capacity(sizeof(Datapoint))) {
				// This should be covered by the above reportable_points
				// check, but just in case break here
				return false;
			}

			Datapoint& point = _cache[reportable_points];
			_last_sent_point = reportable_points;
			reportable_points -= 1;
			buf.append((void*)&point, sizeof(Datapoint));
		}

		return _last_sent_point == 0;
	}

	void reset() {
		_cache_index = 0;
		_last_sent_point = 0;
	}

	// Call if a send failed and points from this sensor need to be resent
	void reset_last_sent() {
		_last_sent_point = 0;
	}

	uint32_t last_sent_point() {
		return _last_sent_point;
	}

	const char* id() {
		return _id;
	}

private:
	uint32_t _cache_index = 0;
	uint32_t _last_sent_point = 0;
	Datapoint _cache[DATAPOINT_CACHE_SIZE];
	const char* _id;
	SensorDriver* _driver;	
};