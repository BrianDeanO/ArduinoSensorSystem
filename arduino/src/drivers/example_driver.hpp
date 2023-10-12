#include "../device_driver.hpp"

class ExampleDevice : public DeviceDriver {
public:
	ExampleDevice(const char* id) : _id(id) {}

	const char* id() override {
		return _id;
	}

	// Request a new data point from the device.
	double acquire_data_point() override {
		return 0.0;
	}

	// The units this device reports in.
	const char* units() override {
		return "C";
	}

private:
	const char* _id;
};