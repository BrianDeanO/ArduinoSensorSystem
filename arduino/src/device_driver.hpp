// Interface for device drivers which report a single
// data point, and the units for that point.
class DeviceDriver {
public:
	// Request a new data point from the device.
	virtual const char* id() = 0;

	// Request a new data point from the device.
	virtual double acquire_data_point() = 0;

	// The units this device reports in.
	virtual const char* units() = 0;
};