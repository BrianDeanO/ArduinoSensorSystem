#include "../../config.hpp"
#include "../sensor.hpp"
#include "../lib/ArduinoJson.h"

class ExampleSensor : public Sensor {
public:
	ExampleSensor(const char* id) : Sensor(id) {}

	virtual bool acquire_channel_value(uint8_t channel, double& value) override
	{
		switch(channel) {
		case 0: 
			value =  fake_value;
			break;
		case 2: 
			value = fake_value / 2;
			break;
		}

		fake_value += 1;
		return true;
	}

	virtual bool read_config(JsonObject config) override { return true; }
	virtual bool write_config(JsonObject config) override { return true; }

	virtual uint8_t channel_count() const override { return 2; }
	virtual const char* channel_units(uint8_t channel) const override
	{
		switch(channel) {
		case 0: return "C";
		case 1: return "atm";
		}
	}

	virtual const char* sensor_type() const { return "example"; }

private:
	double fake_value = 0;
};