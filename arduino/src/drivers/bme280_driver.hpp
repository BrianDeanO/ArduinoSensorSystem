// REQUIRED LIBRARIES:
// https://github.com/adafruit/Adafruit_Sensor
// https://github.com/adafruit/Adafruit_BME280_Library
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
	#endif
	int gain = 0;
	int offset = 0;

public:
	BME280Sensor(const char* id) : Sensor(id) {}
	 
	virtual void init() override
	{
		DEBUG("Initializing BME280 sensor...\n");
		#ifndef SIMULATOR
		if(!bme.begin()) {
			DEBUG("Could not find a valid BME280 sensor!\n");
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
				gain = atoi(config["gain"].as<const char*>());
			if(config.containsKey("offset"))
				offset = atoi(config["offset"].as<const char*>());
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