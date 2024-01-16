#include <Arduino.h>

#include "config.hpp"
#include "src/device.hpp"
#include "src/drivers/example_driver.hpp"

ExampleDriver driver("temp", "Celsius");
Sensor sensors[1] = { Sensor("Temperature 1", &driver) };

#ifndef NO_LTE
    #include "src/lib/SparkFun_LTE_Shield.h"
    #define LTE_POWER_PIN 5
    #define LTE_RESET_PIN 6
    LTE_Shield g_lte(LTE_POWER_PIN, LTE_RESET_PIN);
    LTEClient client(&g_lte);
#else
    DataClient client;
#endif

Device device("test", sensors, 1, &client);


void setup() {
    Serial.begin(9600);
    DEBUG(while(!Serial)); // Wait for Serial (USB Serial connection) to start up for logging
    DEBUG(Serial.println("Program Start"));

#ifndef NO_LTE
    // Use hardware serial port 1 to communicate with the lte shield

    while(true) {
        if(g_lte.begin(Serial1)) {
            Serial.println("Initialized LTE Shield on HW Serial1");
            break;
        }
        else {
            Serial.println("Failed to start LTE Shield on HW Serial1, retrying in 10 seconds.");
            delay(10000);
        }
    }

    g_lte.autoTimeZone(true);
#endif

    device.register_device();
}

void loop() {
    device.update();

    uint16_t ms = device.next_update() - client.get_time();
    DEBUG(Serial.println("Waiting " + String(ms)));
    delay(ms);
#ifdef NO_LTE
    client.fake_last_time += ms;
#endif
}
