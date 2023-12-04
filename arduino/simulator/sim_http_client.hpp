#include "http_client.hpp"

// #define CPPHTTPLIB_OPENSSL_SUPPORT
#include "httplib.h"

#include <string>

class HttpSim : public HttpClient {
public:
    HttpSim(const char* addr, unsigned port) : HttpClient(addr, port) {}

    virtual int get(const char* route, char* response,
                    unsigned max_size) override
    {
        httplib::Client cli(std::string(this->_addr), this->_port);
        auto res = cli.Get(route);
        if (res) {
            std::string body = res->body;
            if (body.size() > max_size) {
                body = body.substr(0, max_size);
            }
            strcpy(response, body.c_str());
            return res->status;
        }
        return -1;
    }

    virtual int post(const char* route, const char* data,
                     const char* content_type, char* response,
                     unsigned max_size) override
    {
        httplib::Client cli(std::string(this->_addr), this->_port);
        auto res = cli.Post(route, data, "application/json");
        if (res) {
            std::string body = res->body;
            if (body.size() > max_size) {
                body = body.substr(0, max_size);
            }
            strcpy(response, body.c_str());
            return res->status;
        }
        return -1;
    }
};