#pragma once

// Compile flags
#define NO_LTE // Compile without LTE support, using serial instead
#define DEBUG_MODE 1 // Enable debug output. Disable to save memory.

// Identifiers /////////////////////////////////
// These identifiers are used to associate physical devices with database entries.
// If a device restarts and loses track of its database id, this is how it gets
// it back. These identifiers should be unique across all devices and sensors.
#define DEVICE_IDENT "devTest1"
#define SENSOR_EXAMPLE_IDENT DEVICE_IDENT "tc1"

// Configuration ///////////////////////////////
#define DEFAULT_DEVICE_TYPE "Test Device"
#define DEFAULT_DEVICE_NAME "Test device 1"

// The interval, in seconds, that the device will acquire new data and send it to
// the server.
#define DEFAULT_UPDATE_INTERVAL "60"

// Interval to poll for configuration in seconds. This is generally shorter than
// the update interval, and is used to check if the server has changed the
// update interval. This value cannot be changed remotely, in order to act as a
// safeguard against accidentally setting the update interval too long.
#define CONFIG_POLL_INTERVAL 30 

// Minimum allowed update interval, to prevent flooding the database with points.
// This value cannot be changed remotely.
#define MIN_UPDATE_INTERVAL 10 

// The address and port of the server to connect to
#define DEFAULT_SERVER_ADDRESS "localhost"
#define DEFAULT_SERVER_PORT 8080

// The number of data points per sensor to store in case of network failure.
// Note that all channels on a sensor share the same cache, a 50 point cache on 
// a sensor with 2 channels will last 25 updates.
#define DATAPOINT_CACHE_SIZE 50

// Buffer size for HTTP responses. This should be large enough to hold the largest response you expect to process.
#define RESPONSE_BUFFER_SIZE 512

////////////////////////////////////////////////