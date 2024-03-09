#include <Arduino.h>

#include "config.hpp"
#include "src/device.hpp"
#include "src/drivers/example_driver.hpp"
#include "src/drivers/bme280_driver.hpp"
#include "src/util.hpp"

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
#else
    // If we're communicating over serial, we need a timestamp to base our fake time on
    Serial.write("REQUEST_TIME\n");
    Serial.flush();
    char buf[32];
    int bytes = Serial.readBytesUntil('\n', buf, 32);
    buf[bytes] = '\0';
    DEBUG("Got timestamp %s\n", buf);

    client.fake_last_time = strtoull(buf, nullptr);
    if(client.fake_last_time == 0) {
        DEBUG("Failed to parse time from serial, using 0\n");
    }
    else {
        DEBUG("Got time from serial: %llu\n", client.fake_last_time);
    }
#endif

    device.init();

    digitalWrite(LED_BUILTIN, HIGH);
}

void loop() {
    time_t current_time = client.get_time();
    DEBUG("Current time: %lu\n", current_time);

	device.poke_device(); // Send a "poke" to the server to let it know we're still alive
    device.get_config(); // Update config in case settings have changed since our last poll
    device.update(current_time); // Will send data to the server if current_time is past the next update time

    // Wait a maximum of CONFIG_POLL_INTERVAL ms
    uint32_t duration = (device.next_update() - current_time);
    if(duration > CONFIG_POLL_INTERVAL || duration < 0)
        duration = CONFIG_POLL_INTERVAL;
    DEBUG("Waiting %d seconds\n", duration);
    delay(duration * 1000);

#ifdef NO_LTE
    client.fake_last_time += duration;
#endif
}
