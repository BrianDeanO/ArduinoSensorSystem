#pragma once

// Compile flags
#define NO_LTE
#define DEBUG_MODE 1

// Identifiers
#define DEVICE_IDENT "devTest1"
#define SENSOR_EXAMPLE_IDENT DEVICE_IDENT "tc1"

// Configuration ///////////////////////////////
#define DEFAULT_DEVICE_TYPE "Test Device"
#define DEFAULT_DEVICE_NAME "Test device 1"
#define DEFAULT_UPDATE_INTERVAL "60" // 60 seconds

// Interval to poll for configuration, in case the data acquisition rate has changed
#define CONFIG_POLL_INTERVAL 30 
// Minimum allowed update interval, to prevent accidentally flooding the database with points
#define MIN_UPDATE_INTERVAL 10 
#define DEFAULT_SERVER_ADDRESS "localhost"
#define DEFAULT_SERVER_PORT 8080
#define DATAPOINT_CACHE_SIZE 50

#define RESPONSE_BUFFER_SIZE 512

////////////////////////////////////////////////
#ifndef SIMULATOR
	#include "Arduino.h"
	#define LTE_SHIELD_HW_SERIAL Serial1
#else
	#include <cstdint>
	#include <cstring>
	#include <unistd.h>
	#define delay(x) usleep(x * 1000)

	// Arduino's copy strings to program memory unless you use the F macro. In simulator mode this is a no-op
	#define F(str) str
#endif

#if DEBUG_MODE == 1 && !defined(SIMULATOR)
	extern char _dbg_msg[256];

	// Print a debugging message, in the form of a printf.
	// __VA_ARGS__ is a macro that expands to the arguments passed to the macro, the ## preceding it removes the comma if there are no arguments.
	// The F macro is used to store the string in program memory, which is read-only and saves RAM.
	// The sprintf_P function is used to print a string from program memory.
	#define DEBUG(format, ...) sprintf_P(_dbg_msg, (const char*)F(format), ## __VA_ARGS__); Serial.print(_dbg_msg)

	#define DEBUG_EXPR(expr) expr
	#define LOGGING // For HttpClient
#elif defined(SIMULATOR)
	#define DEBUG(...) printf(__VA_ARGS__)
	#define DEBUG_EXPR(expr)
#else
	#define DEBUG(...)
	#define DEBUG_EXPR(expr)
#endif

#define ARDUINOJSON_USE_LONG_LONG 1