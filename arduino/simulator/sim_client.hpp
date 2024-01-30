#include "dataClient.hpp"
#include "httplib.h"

class SimClient : public DataClient {
public:
	SimClient(const char* addr, unsigned port) 
		: http(addr, port) {}

    int get(const char* url, char* response, unsigned response_size) override;
    int post(const char* url, const char* body, char* response, unsigned response_size) override;
    int put(const char* url, const char* body, char* response, unsigned response_size) override;
    uint64_t get_time() override;

private:
    int handle_result(httplib::Result res, char* response, unsigned response_size);
    httplib::Client http;
};