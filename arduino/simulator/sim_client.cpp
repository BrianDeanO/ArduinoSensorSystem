#include "sim_client.hpp"
#include <string.h>
#include <time.h>

int SimClient::handle_result(httplib::Result res, char* response, unsigned response_size)
{
	if (res) {
		if(res->status / 100 != 2) {
			if(res->status == 400) {
				DEBUG("Bad request: %s\n", res->body.c_str());
			}
			return -res->status; // Bad status code
		}

		if(!response)
			return 0;

		strncpy(response, res->body.c_str(), response_size);
		DEBUG("Response: %s\n", response);
		return res->body.size();
	}
	return -1;
}

int SimClient::get(const char* url, char* response, unsigned response_size)
{
	auto res = http.Get(url);
	return handle_result(std::move(res), response, response_size);
}

int SimClient::post(const char* url, const char* body, char* response, unsigned response_size)
{
	if(body) {
		auto res = http.Post(url, body, "application/json");
		return handle_result(std::move(res), response, response_size);
	}
	else {
		auto res = http.Post(url);
		return handle_result(std::move(res), response, response_size);
	}
}

int SimClient::put(const char* url, const char* body, char* response, unsigned response_size)
{
	if(body) {
		auto res = http.Put(url, body, "application/json");
		return handle_result(std::move(res), response, response_size);
	}
	else {
		auto res = http.Put(url);
		return handle_result(std::move(res), response, response_size);
	}
}

uint64_t SimClient::get_time() {
	return time(NULL);
}