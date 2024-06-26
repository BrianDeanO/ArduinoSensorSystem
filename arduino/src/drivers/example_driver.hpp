#pragma once
#include "../../common.hpp"
#include "../sensor.hpp"
#include "../lib/ArduinoJson.h"

class ExampleSensor : public Sensor {
public:
	ExampleSensor(const char* id) : Sensor(id) {}
	virtual void init() override { fake_value = 0; }

	virtual bool acquire_channel_value(uint8_t channel, double& value) override
	{
		switch(channel) {
		case 0: 
			value =  fake_value;
			break;
		case 1: 
			value = fake_value / 2;
			break;
		}

		fake_value += incrementer;
		return true;
	}

	virtual bool read_config(JsonObject& config) override { 
		char buf[10];
		sprintf(buf, "%d", incrementer);
		config["incrementer"] = buf;
		return true;
	}

	virtual bool write_config(JsonObject config) override { 
		if(config.containsKey("incrementer")) {
			incrementer = atoi(config["incrementer"].as<const char*>());
		}
		return true;
	}

	virtual uint8_t channel_count() const override { return 2; }
	virtual const char* channel_units(uint8_t channel) const override
	{
		switch(channel) {
		case 0: return "C";
		case 1: return "atm";
		}
		return "unknown";
	}

	virtual const char* sensor_type() const { return "example"; }

private:
	double fake_value = 0;
	int incrementer = 1;
};