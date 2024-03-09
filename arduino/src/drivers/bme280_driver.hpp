#include "../../config.hpp"
#include "../sensor.hpp"
#include "../lib/ArduinoJson.h"

#include <Adafruit_BME280.h>

class BME280Sensor : public Sensor {
	Adafruit_BME280 bme;

public:
	BME280Sensor(const char* id) : Sensor(id) {}
	 
	virtual void init() override
	{
		DEBUG("Initializing BME280 sensor...\n");
		if(!bme.begin()) {
			DEBUG("Could not find a valid BME280 sensor!\n");
		}
	}

	virtual bool acquire_channel_value(uint8_t channel, double& value) override
	{
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

		return true;
	}

	virtual bool read_config(JsonObject config) override { return true; }
	virtual bool write_config(JsonObject config) override { return true; }

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