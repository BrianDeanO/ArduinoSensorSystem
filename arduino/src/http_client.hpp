#pragma once

#include <cstdio>

#define HTTP_REQUEST_HEADER " %s HTTP/1.1\nHost: %s\n\n"

class HttpClient {
  public:
    virtual int get(const char* addr, unsigned port, const char* route,
                    char* response, unsigned max_size) = 0;
    virtual int post(const char* addr, unsigned port, const char* route,
                     const char* body, const char* content_type, char* response,
                     unsigned max_size) = 0;
};
