#pragma once

#define NO_LTE
#define DEBUG_MODE 1

#if DEBUG_MODE == 1 && !defined(SIMULATOR)
	#define DEBUG(expr) expr
#else
	#define DEBUG(expr)
#endif

#define DATAPOINT_CACHE_SIZE 50
#define SEND_BUFFER_SIZE 512
#define DEFAULT_RECORD_INTERVAL 5000
#define DEFAULT_SERVER_ADDRESS "localhost"
#define DEFAULT_SERVER_PORT 80

#ifndef SIMULATOR
	#include "Arduino.h"
	#define LTE_SHIELD_HW_SERIAL Serial1
#else
	#include <cstdint>
	#include <cstring>
#endif