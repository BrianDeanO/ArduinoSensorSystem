#include <Arduino.h>

#include "src/device.hpp"
#include "src/drivers/example_driver.hpp"

TimeClient time_client;
// HttpClient http_client;

ExampleDriver driver("temp");
Sensor sensors[1] = { Sensor("Temperature 1", &driver) };
Device device("test", sensors, 1, &http_client, &time_client);

void setup() {
    device.register_device();
}

void loop() {
    device.update();
    delay(device.next_update() - time_client.get_time());
}
