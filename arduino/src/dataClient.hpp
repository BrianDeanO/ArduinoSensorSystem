#pragma once
#include "../config.hpp"

class DataClient {
public:
    // Send an HTTP GET request to `url`, placing response text into `response`.
    // Returns the number of bytes provided in response, or a negative value
    // reflecting the HTTP status code if the request failed (e.g. -404 for a 404 error)
    virtual int get(const char* url, char* response, unsigned response_size) = 0;
    virtual int post(const char* url, const char* body, char* response, unsigned response_size) = 0;
    virtual int put(const char* url, const char* body, char* response, unsigned response_size) = 0;
    virtual uint64_t get_time() = 0;
};

#ifndef SIMULATOR
#include <ArduinoHttpClient.h>
#include "serialClient.hpp"
#include "lteClient.hpp"
#include "time.hpp"
#include "lib/SparkFun_LTE_Shield.h"

class SerialDataClient : public DataClient {
public:
    SerialDataClient(HardwareSerial* serial, const char* addr, uint16_t port) 
        : addr(addr), port(port), serial_client(serial), http(serial_client, addr, port) {}

    int get(const char* url, char* response, unsigned response_size) override;
    int post(const char* url, const char* body, char* response, unsigned response_size) override;
    int put(const char* url, const char* body, char* response, unsigned response_size) override;
    uint64_t get_time() override { return fake_last_time; }

    uint64_t fake_last_time = 0;
    SerialClient serial_client;
    HttpClient http;
    const char* addr;
    uint16_t port;
};

class LTEDataClient : public DataClient {
public:
    LTEDataClient(LTE_Shield* lte, const char* addr, unsigned port)
        : lte(lte), lte_client(lte), http(lte_client, addr, port) {}

    int get(const char* url, char* response, unsigned response_size) override;
    int post(const char* url, const char* body, char* response, unsigned response_size) override;
    int put(const char* url, const char* body, char* response, unsigned response_size) override;
    uint64_t get_time() override;

    LTE_Shield* lte;
    LTEClient lte_client;
    HttpClient http;
};

#endif