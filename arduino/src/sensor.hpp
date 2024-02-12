#pragma once
#include "../config.hpp"
#include "lib/ArduinoJson.h"
#include "dataClient.hpp"

struct Datapoint {
	double value;
	uint64_t time;
	uint8_t channel;
};

class Sensor {
public:
	Sensor(const char* identifier) {
		this->_ident = identifier;
	}

	uint32_t id() const { return _id; }
	const char* ident() const { return _ident; }

	virtual bool acquire_channel_value(uint8_t channel, double& value) = 0;
	virtual bool read_config(JsonObject config) = 0;
	virtual bool write_config(JsonObject config) = 0;

	virtual uint8_t channel_count() const = 0;
	virtual const char* channel_units(uint8_t channel) const = 0;
	virtual const char* sensor_type() const = 0;

	// Acquire a new data point from the sensor, storing it in this sensor's cache.
	// Use pop_data_point() to retrieve the data point.
	virtual void cache_data_point(uint64_t time);
	bool get_last_point(Datapoint& point);
	void pop_last_point();
	void reset();

	bool register_sensor(DataClient* client, char* buf, uint16_t buf_size, uint32_t deviceID);

private:
	const char* _ident;
	uint32_t _id = 0;
	uint16_t _cache_index = 0;
	Datapoint _cache[DATAPOINT_CACHE_SIZE];
};