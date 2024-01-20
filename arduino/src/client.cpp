#include "client.hpp"

bool DataClient::send(const char* command, uint32_t size) {
	DEBUG(
		Serial.print("COMMAND BYTES: ");
		char* c = command;
		while(*c) {
			Serial.print(*c, HEX);
			Serial.print(" ");
			c += 1;
		}
		Serial.println();
	)

	Serial.print("COMMAND: [" + String(size) + "]:");
	Serial.write(command, size);
}

uint64_t DataClient::get_time() {
	#ifdef SIMULATOR
		return time(NULL);
	#else
		return fake_last_time;
	#endif
}

#ifndef NO_LTE
LTEClient::LTEClient(LTE_Shield* lte) : lte(lte) {}

bool LTEClient::send(const char* command, uint32_t size) {
	LTE_Shield_error_t err;
	int socket = lte->socketOpen(LTE_SHIELD_TCP);

	err = lte->socketConnect(socket, DEFAULT_SERVER_ADDRESS, DEFAULT_SERVER_PORT);
	if(err != LTE_SHIELD_ERROR_SUCCESS){
		Serial.println("Error connecting during send: LTE Error " + String(err));
	}

	err = lte->socketWrite(socket, command, size);
	if(err != LTE_SHIELD_ERROR_SUCCESS){
		Serial.println("Error writing to socket during send: LTE Error " + String(err));
	}

	err = lte->socketClose(socket);
	if(err != LTE_SHIELD_ERROR_SUCCESS){
		Serial.println("Error closing socket after sending command: LTE Error " + String(err));
	}
}

uint64_t LTEClient::get_time() {
	tmElements_t tm;
	uint8_t tz;
	LTE_Shield_error_t err = lte->clock(&tm.Year, &tm.Month, &tm.Day, &tm.Hour, &tm.Minute, &tm.Second, &tz);
	if(err != LTE_SHIELD_ERROR_SUCCESS) {
		Serial.println("Error getting lte time: " + String(err));
		return 0;
	}
	else {
		return makeTime(tm);
	}
}

#endif