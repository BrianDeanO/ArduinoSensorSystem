#pragma once

#include <cstdio>

#define HTTP_REQUEST_HEADER " %s HTTP/1.1\nHost: %s\n\n"

class HttpClient {
  public:
    HttpClient(const char* addr, unsigned port) : _addr(addr), _port(port) {}

    virtual int get(const char* route, char* response, unsigned max_size) = 0;
    virtual int post(const char* route, const char* body,
                     const char* content_type, char* response,
                     unsigned max_size) = 0;

protected:
    const char* _addr;
    unsigned _port;
};
