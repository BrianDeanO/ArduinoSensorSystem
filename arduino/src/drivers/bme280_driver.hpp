// REQUIRED LIBRARIES:
// https://github.com/adafruit/Adafruit_Sensor
// https://github.com/adafruit/Adafruit_BME280_Library
// Dependencies for SoftWire:
//     https://github.com/stevemarple/AsyncDelay

#pragma once
#include "../../common.hpp"
#include "../sensor.hpp"
#include "../lib/ArduinoJson.h"

#ifndef SIMULATOR
#include <Adafruit_BME280.h>
#else
#include <string>
#endif

class BME280Sensor : public Sensor {
	#ifndef SIMULATOR
	Adafruit_BME280 bme;
	TwoWire* wire;
	uint8_t addr;
	#endif
	float gain = 1;
	float offset = 0;

public:
#ifndef SIMULATOR
	BME280Sensor(const char* id, uint8_t addr = BME280_ADDRESS, TwoWire* wire = nullptr) : Sensor(id), addr(addr), wire(wire) {}
#else
	BME280Sensor(const char* id) : Sensor(id) {}
#endif
	 
	virtual void init() override
	{
		#ifndef SIMULATOR
		bool result;
		DEBUG("Connecting to BME280 sensor at 0x%02X\n", addr);
		if(wire) {
			result = bme.begin(addr, wire);
		} else {
			result = bme.begin(addr);
		}
		if(!result) {
			DEBUG("Could not find a valid BME280 sensor! (%s)\n", ident());
		}
		#endif
	}

	virtual bool acquire_channel_value(uint8_t channel, double& value) override
	{
		#ifndef SIMULATOR
		switch(channel) {
		case 0: 
			value = bme.readTemperature();
			break;
		case 1: 
			value = bme.readPressure() / 1000.0F;
			break;
		case 2: 
			value = bme.readHumidity();
			break;
		}
		#else
		value = 0;
		#endif

		value *= gain;
		value += offset;

		return true;
	}

	virtual bool read_config(JsonObject& config) override { 
		#ifndef SIMULATOR
			config["gain"] = String(gain);
			config["offset"] = String(offset);
		#else
			config["gain"] = std::to_string(gain);
			config["offset"] = std::to_string(offset);
		#endif
		return true;
	}
	virtual bool write_config(JsonObject config) override {
		#ifndef SIMULATOR
			if(config.containsKey("gain"))
				gain = config["gain"].as<String>().toInt();
			if(config.containsKey("offset"))
				offset = config["offset"].as<String>().toInt();
		#else
			if(config.containsKey("gain"))
				gain = atof(config["gain"].as<const char*>());
			if(config.containsKey("offset"))
				offset = atof(config["offset"].as<const char*>());
		#endif

		return true;
	}

	virtual uint8_t channel_count() const override { return 3; }
	virtual const char* channel_units(uint8_t channel) const override
	{
		switch(channel) {
			case 0: return "C";
			case 1: return "kPa";
			case 2: return "% humidity";
		}
		return "unknown";
	}

	virtual const char* sensor_type() const { return "Adafruit BME280"; }
};