#include <Arduino.h>

#include "config.hpp"
#include "src/device.hpp"
#include "src/drivers/example_driver.hpp"
char _dbg_msg[256]; // Used in the DEBUG macros, declared in config.hpp

ExampleSensor sen1("demo_sensor1");
Sensor* sensors[] = { &sen1 };

#ifndef NO_LTE
    #include "src/lib/SparkFun_LTE_Shield.h"
    #define LTE_POWER_PIN 5
    #define LTE_RESET_PIN 6
    LTE_Shield g_lte(LTE_POWER_PIN, LTE_RESET_PIN);
    LTEDataClient client(&g_lte, DEFAULT_SERVER_ADDRESS, DEFAULT_SERVER_PORT);
#else
    SerialDataClient client(&Serial, DEFAULT_SERVER_ADDRESS, DEFAULT_SERVER_PORT);
#endif

Device device(sensors, 1, &client);

void setup() {
    Serial.begin(9600);
    DEBUG_EXPR(while(!Serial)); // Wait for Serial (USB Serial connection) to start up for logging
    DEBUG("Program Start\n");

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
    DEBUG("Waiting %d\n", ms);
    delay(ms);
#ifdef NO_LTE
    client.fake_last_time += ms;
#endif
}
