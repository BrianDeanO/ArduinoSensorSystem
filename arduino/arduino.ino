#include <Arduino.h>

#include "config.hpp"
#include "src/device.hpp"
#include "src/drivers/example_driver.hpp"
#include "src/drivers/bme280_driver.hpp"

#if DEBUG_MODE == 1
char _dbg_msg[256]; // Used in the DEBUG macros, declared in config.hpp
#endif

BME280Sensor sen1("bme280_sensor1");
// ExampleSensor sen1("demo_sensor1");

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
    pinMode(LED_BUILTIN, OUTPUT);
    digitalWrite(LED_BUILTIN, LOW);

    Serial.begin(9600);
    DEBUG_EXPR(while(!Serial)); // Wait for Serial (USB Serial connection) to start up for logging
    DEBUG("Program Start\n");

#ifndef NO_LTE
    // Use hardware serial port 1 to communicate with the lte shield

    while(true) {
        if(g_lte.begin(Serial1)) {
            Serial.println(F("Initialized LTE Shield on HW Serial1"));
            break;
        }
        else {
            Serial.println(F("Failed to start LTE Shield on HW Serial1, retrying in 10 seconds."));
            delay(10000);
        }
    }

    g_lte.autoTimeZone(true);
#endif

    device.init();

    digitalWrite(LED_BUILTIN, HIGH);
}

void loop() {
    device.get_config(); // Update config in case settings have changed since our last poll
    // If the next update time is in the past, update now.
    if(device.next_update() <= client.get_time()) {
        device.update();
    }

    // Wait a maximum of CONFIG_POLL_INTERVAL ms
    uint32_t duration = (device.next_update() - client.get_time());
    if(duration > CONFIG_POLL_INTERVAL)
        duration = CONFIG_POLL_INTERVAL;
    DEBUG("Waiting %d seconds\n", duration);
    delay(duration * 1000);

#ifdef NO_LTE
    client.fake_last_time += duration;
#endif
}
