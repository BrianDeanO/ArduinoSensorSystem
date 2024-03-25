#include <iostream>

#include "../common.hpp"
#include "../sensor_definitions.hpp"
#include "sim_client.hpp"
#include "device.hpp"
#include "drivers/example_driver.hpp"

#include <unistd.h>
#include <vector>

class TestSensor : public Sensor {
public:
	TestSensor(const char* identifier) : Sensor(identifier) {}

	void init() override {
		std::cout << "TestSensor::init()" << std::endl;
	}

	bool acquire_channel_value(uint8_t channel, double& value) override {
		std::cout << "TestSensor::acquire_channel_value(" << (int)channel << ")" << std::endl;
		switch(channel) {
		case 0:
			value = 42.0;
			return true;
		case 1:
			value = 3.14;
			return true;
		}

		return false;
	}

	bool read_config(JsonObject& config) override {
		std::cout << "TestSensor::read_config()" << std::endl;
		return true;
	}

	bool write_config(JsonObject config) override {
		std::cout << "TestSensor::write_config()" << std::endl;
		return true;
	}

	uint8_t channel_count() const override {
		return 2;
	}

	const char* channel_units(uint8_t channel) const override {
		switch(channel) {
		case 0:
			return "units";
		case 1:
			return "radians";
		}

		return "";
	}

	const char* sensor_type() const override {
		return "test";
	}
};

int main() {
	std::string addr = "localhost";
	unsigned port = 8080;

    httplib::Client http(addr, port);
	if(!http.is_valid()) {
		std::cout << "Failed to connect to server." << std::endl;
		return 1;
	}

	SimClient client(addr.c_str(), port);
	TestSensor* sensor = new TestSensor("testSensor");
	std::vector<Sensor*> sensors { sensor };
	Device device(sensors.data(), 1, &client);

	device.init();
	auto res = http.Get("/api/Device");
	std::cout << "Request: GET /api/Device" << std::endl;
	std::cout << "Response: " << res->body << std::endl;
	assert(res && res->status / 100 == 2);

	JsonDocument doc;
	auto err = deserializeJson(doc, res->body);
	assert(err == DeserializationError::Ok);
	assert(doc.is<JsonArray>() && doc.size() > 0);
	assert(doc[0].is<JsonObject>());
	assert(doc[0].containsKey("deviceID") && doc[0]["deviceID"].is<int>());
	assert(doc[0].containsKey("deviceIdent") && doc[0]["deviceIdent"].is<std::string>());
	assert(doc[0].containsKey("deviceName") && doc[0]["deviceName"].is<std::string>());
	assert(doc[0].containsKey("deviceType") && doc[0]["deviceType"].is<std::string>());
	assert(doc[0].containsKey("deviceUpdateInterval") && doc[0]["deviceUpdateInterval"].is<int>());

	assert(doc[0]["deviceIdent"].as<std::string>() == DEVICE_IDENT);
	assert(doc[0]["deviceName"].as<std::string>() == DEFAULT_DEVICE_NAME);
	assert(doc[0]["deviceType"].as<std::string>() == DEFAULT_DEVICE_TYPE);
	assert(doc[0]["deviceUpdateInterval"] == atoi(DEFAULT_UPDATE_INTERVAL));

	int newUpdateInterval = 90;
	res = http.Put("/api/Device/" + std::to_string(device.id()), "{\"deviceUpdateInterval\": " + std::to_string(newUpdateInterval) + "}", "application/json");
	std::cout << "Request: PUT /api/Device/" << device.id() << " {\"deviceUpdateInterval\": " << newUpdateInterval << "}" << std::endl;
	std::cout << "Response: " << res->body << std::endl;
	assert(res && res->status / 100 == 2);

	if(!device.poke_device()) {
		std::cout << "Failed to poke device." << std::endl;
		return 1;
	}

	if(!device.get_config()) {
		std::cout << "Failed to get config." << std::endl;
		return 1;
	}

	assert(device.update_interval() == newUpdateInterval);

	time_t current_time = client.get_time();
	device.update(current_time);

	res = http.Get("/api/SensorData");
	std::cout << "Request: GET /api/SensorData" << std::endl;
	std::cout << "Response: " << res->body << std::endl;
	assert(res && res->status / 100 == 2);
	
	err = deserializeJson(doc, res->body);
	assert(err == DeserializationError::Ok);
	assert(doc.is<JsonArray>() && doc.size() == 2); // We have two channels
	assert(doc[0].is<JsonObject>());
	assert(doc[0].containsKey("channelID") && doc[0]["channelID"].is<int>());
	assert(doc[0].containsKey("sensorID") && doc[0]["sensorID"].is<int>());

	assert(doc[0]["sensorID"] == sensor->id());

	std::cout << "\n    All tests passed." << std::endl;
}