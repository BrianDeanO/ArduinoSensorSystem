#include "../driver.hpp"

class ExampleDriver : public SensorDriver {
public:
	ExampleDriver(const char* id, const char* units) : _id(id), _units(units) {}

	const char* sensor_type_id() override {
		return _id;
	}

	// Request a new data point from the device.
	double acquire_data_point() override {
		return 0.0;
	}

	// The units this device reports in.
	const char* units() override {
		return _units;
	}

private:
	const char* _id;
	const char* _units;
};