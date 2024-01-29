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
#define DEFAULT_SERVER_PORT 80

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
	char _dbg_msg[256];
	#define DEBUG(...) sprintf(_dbg_msg, __VA_ARGS__); Serial.write(_dbg_msg, 256)
#else if defined(SIMULATOR)
	#define DEBUG(...) printf(_dbg_msg, __VA_ARGS__)
#else
	#define DEBUG(...)
#endif
