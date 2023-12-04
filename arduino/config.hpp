#pragma once

#define DATAPOINT_CACHE_SIZE 50
#define SEND_BUFFER_SIZE 512
#define DEFAULT_RECORD_INTERVAL 3600
#define DEFAULT_SERVER_ADDRESS "localhost"
#define DEFAULT_SERVER_PORT 80

#ifndef SIMULATOR
	#include "Arduino.h"
	#define LTE_SHIELD_HW_SERIAL Serial1
#endif