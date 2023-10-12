#include "src/drivers/example_driver.hpp"

unsigned record_interval = 3600; // 1 hour
unsigned retry_interval = 300; // 5 minutes
unsigned device_count = 2;
DeviceDriver* devices[device_count];

unsigned cache_size = 100;
unsigned cache_index = 0;
double cache[device_count][cache_size];
time_t cache_times[cache_size];

bool send_cached_data() {
    // TODO
}

bool reconnect() {
    // TODO
}

void record_data() {
    for(unsigned i = 0; i < device_count; i++) {
        DeviceDriver* device = devices[i];
        double value = device->acquire_data_point();
        if(isnan(value)) {
            Serial.print("Error: device ");
            Serial.print(device->id());
            Serial.println(" returned NaN.");
        }

        cache[i][cache_index] = value;
    }

    cache_index++;
}

// the setup function runs once when you press reset or power the board
void setup() {
    devices[0] = new ExampleDevice("tc1"); // Thermocouple 1
    devices[1] = new ExampleDevice("lum1"); // Luminosity 1
}

// the loop function runs over and over again forever
void loop() {
    record_data();

    unsigned elapsed_retry_time = 0;
    while(!send_cached_data()) {
        delay(retry_interval); // Wait before trying to reconnect
        elapsed_retry_time += retry_interval;
        if(elapsed_retry_time > record_interval) {
            // Keep recording event if we can't connect
            record_data();
            elapsed_retry_time = 0;
        }
    }

    delay(record_interval - total_retry_time);
}
