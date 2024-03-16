// This file is separate from arduino.ino to make it easier to manage for each
// device.

// Sensor Definitions ////////////////////////////////////////////////
// Below, you can define and configure your sensors. Each sensor should have a 
// unique name, and must be added to the sensors array.

#include "src/drivers/bme280_driver.hpp"
#include "src/drivers/example_driver.hpp"

BME280Sensor sen1(DEVICE_IDENT "_bme280");
ExampleSensor sen2(DEVICE_IDENT "_example");

Sensor* sensors[] = { &sen1 };
const unsigned NUM_SENSORS = 1;