#pragma once

#ifndef SIMULATOR

#include "HardwareSerial.h"
#include "Client.h"
#include <string.h>

class SerialClient : public Client {
public:
	SerialClient(HardwareSerial* serial)
	{
		this->serial = serial;
	}

	virtual int connect(IPAddress ip, uint16_t port) override
	{
		// char addr[16];
		// sprintf(addr, "%d.%d.%d.%d", ip[0], ip[1], ip[2], ip[3]);
		// return connect(addr, port);
		return 1;
	}

	virtual int connect(const char* host, uint16_t port) override
	{
		// char buf[36];
		// sprintf("CONNECT:%s:%d\n", host, port);
		// serial->write(buf, 36);
		return 1;
	}

	virtual size_t write(uint8_t byte) override
	{
		return serial->write(byte);
	}

	virtual size_t write(const uint8_t *buf, size_t size) override
	{
		return serial->write(buf, size);
	}

	virtual int read() override
	{
		int result = serial->read();
		// Serial.print(String("byte: ") + (char)result + " | " + String(result) + "\n");
		return result;
	}

	virtual int read(uint8_t *buf, size_t size) override
	{
		return serial->readBytes(buf, size);
	}

	virtual int peek() override
	{
		return serial->peek();
	}

	virtual void flush() override
	{
		serial->flush();
	}

	virtual void stop() override
	{
	}

	virtual uint8_t connected() override { return true; }
	virtual operator bool() { return connected(); };

	virtual int available() override
	{
		const int attempts = 5;
		const int byte_threshold = 5;
		// DEBUG("DBG: available: %d\n", serial->available());

		// The board has a small buffer for bytes written to serial,
		// so the proxy writes in small chunks of bytes. Once we've
		// read all the bytes in the buffer, we request more by sending
		// a "MORE PLS" line to the proxy.
		if (!requested_more && serial->available() < byte_threshold) {
			int timeout = attempts;
			while(serial->available() < byte_threshold && timeout > 0) {
				delay(100);
				timeout -= 1;
			}
			// If we timed out, request more bytes
			if(timeout <= 0) {
				// serial->flush();
				serial->print("MORE PLS\n");
				requested_more = true;
			}
		}
		else if(serial->available() >= byte_threshold) {
			requested_more = false;
		}

		return serial->available();
	}

	void flush_input() {
		while(serial->available()) {
			serial->read();
		}
	}

private:
	HardwareSerial* serial = nullptr;
	bool requested_more = false;
};

#endif // SIMULATOR