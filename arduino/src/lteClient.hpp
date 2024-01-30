#pragma once

#include "Client.h"
#include "lib/SparkFun_LTE_Shield.h"

#define CLIENT_BUFFER_SIZE 1024

class LTEClient : public Client {
public:
	LTEClient(LTE_Shield* shield) : shield(shield) {}

	virtual int connect(IPAddress ip, uint16_t port) override
	{
		char addr[16];
		sprintf(addr, "%d.%d.%d.%d", ip[0], ip[1], ip[2], ip[3]);
		return connect(addr, port);
	}

	virtual int connect(const char* host, uint16_t port) override
	{
		socket = shield->socketOpen(LTE_SHIELD_TCP);
		shield->socketConnect(socket, host, port);
		return 0;
	}

	virtual size_t write(uint8_t byte)
	{
		write_buf[write_cursor] = byte;
		write_cursor += 1;
		if(write_cursor >= CLIENT_BUFFER_SIZE) {
			this->flush();
		}
		return 1;
	}

	virtual size_t write(const uint8_t *buf, size_t size)
	{
		if(write_cursor + size < CLIENT_BUFFER_SIZE) {
			memcpy(write_buf + write_cursor, buf, size);
			write_cursor += size;
		}
		else {
			this->flush();
			shield->socketWrite(socket, (char*)buf, size);
		}
		return size; 
	}

	virtual int available()
	{
		return -1;
	}

	virtual int read()
	{
		if(has_peeked_byte) {
			has_peeked_byte = false;
			return peeked_byte;
		}

		uint8_t buf;
		if(this->read(&buf, 1) >= 0)
			return buf;
		else
			return -1;
	}

	virtual int read(uint8_t *buf, size_t size)
	{
		if(has_peeked_byte && size >= 1) {
			buf[0] = peeked_byte;
			buf += 1;
			size -= 1;
			has_peeked_byte = false;
		}

		memset(buf, 0, size);
		if(shield->socketRead(socket, size, buf) != LTE_SHIELD_ERROR_SUCCESS) {
			return -1;
		}
		return strlen(buf);
	}

	virtual int peek()
	{
		peeked_byte = this->read();
		has_peeked_byte = true;
		return peeked_byte;
	}

	virtual void flush()
	{
		shield->socketWrite(socket, (char*)write_buf, write_cursor);
		write_cursor = 0;
	}

	virtual void stop()
	{
		shield->socketClose(socket);
	}

	virtual uint8_t connected() { return socket >= 0; }
	virtual operator bool() { return connected(); };

private:
	LTE_Shield* shield = nullptr;
	int socket = -1;

	bool has_peeked_byte = false;
	int peeked_byte;

	uint8_t write_buf[CLIENT_BUFFER_SIZE];
	uint16_t write_cursor = 0;
};