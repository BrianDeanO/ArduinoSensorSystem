#pragma once

#ifndef SIMULATOR

#include "HardwareSerial.h"
#include "Client.h"

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
		return 0;
	}

	virtual int connect(const char* host, uint16_t port) override
	{
		// char buf[36];
		// sprintf("CONNECT:%s:%d\n", host, port);
		// serial->write(buf, 36);
		return 0;
	}

	virtual size_t write(uint8_t byte)
	{
		return serial->write(byte);
	}

	virtual size_t write(const uint8_t *buf, size_t size)
	{
		return serial->write(buf, size);
	}

	virtual int available()
	{
		return serial->available();
	}

	virtual int read()
	{
		return serial->read();
	}

	virtual int read(uint8_t *buf, size_t size)
	{
		return serial->readBytes(buf, size);
	}

	virtual int peek()
	{
		return serial->peek();
	}

	virtual void flush()
	{
		serial->flush();
	}

	virtual void stop() {}

	virtual uint8_t connected() { return true; }
	virtual operator bool() { return connected(); };

private:
	HardwareSerial* serial = nullptr;
};

#endif // NO_LTE