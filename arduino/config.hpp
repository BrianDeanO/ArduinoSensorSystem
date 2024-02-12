#pragma once

// Compile flags
#define NO_LTE
#define DEBUG_MODE 1

// Identifiers
#define DEVICE_IDENT "devTest1"
#define SENSOR_EXAMPLE_IDENT DEVICE_IDENT "tc1"
#define DEFAULT_DEVICE_TYPE "Test Device"
#define DEFAULT_DEVICE_NAME "Test device 1"

// Configuration
#define DATAPOINT_CACHE_SIZE 50
#define DEFAULT_RECORD_INTERVAL 5000
#define DEFAULT_SERVER_ADDRESS "localhost"
#define DEFAULT_SERVER_PORT 8080

#define RESPONSE_BUFFER_SIZE 512

// More
#ifndef SIMULATOR
	#include "Arduino.h"
	#define LTE_SHIELD_HW_SERIAL Serial1
#else
	#include <cstdint>
	#include <cstring>
#endif

#if DEBUG_MODE == 1 && !defined(SIMULATOR)
	extern char _dbg_msg[256];
	#define DEBUG(...) sprintf(_dbg_msg, __VA_ARGS__); Serial.print(_dbg_msg)
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