#pragma once
#include "../common.hpp"
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

	// Initializes the sensor, called once at startup.
	virtual void init() = 0;
	// Acquire a value from the sensor's channel `channel`, storing it in `value`.
	// Returns true if the value was successfully acquired, false otherwise.
	// If the specified channel does not exist, returns false.
	virtual bool acquire_channel_value(uint8_t channel, double& value) = 0;

	// Get current configuration settings from a JSON object.
	// Returns true if the configuration was successfully read, false otherwise.
	// Returns false if the sensor has no configuration to report.
	virtual bool read_config(JsonObject& config) = 0;
	// Apply new configuration settings from a JSON object.
	// Returns true if the configuration was successfully written, false otherwise.
	virtual bool write_config(JsonObject config) = 0;

	// Get the number of channels this sensor has.
	virtual uint8_t channel_count() const = 0;
	// Get the unit string of the channel `channel`.
	virtual const char* channel_units(uint8_t channel) const = 0;
	// Get an identifier for the sensor type.
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