#include "sim_client.hpp"
#include <string.h>
#include <time.h>

int SimClient::handle_response(httplib::Response res)
{
	if (res) {
		if(res->status / 100 != 2) {
			return -res->status; // Bad status code
		}

		strncpy(response, res->body.c_str(), response_size);
		return res->body.size();
	}
	return -1;
}

int SimClient::get(const char* url, char* response, unsigned response_size)
{
	auto res = http.Get(route);
	return handle_response(res);
}

int SimClient::post(const char* url, const char* body, char* response, unsigned response_size)
{
	http.Post(url, body, "application/json");
	return handle_response(res);
}

int SimClient::put(const char* url, const char* body, char* response, unsigned response_size)
{
	http.Put(url, body, "application/json");
	return handle_response(res);
}

uint64_t SimClient::get_time() {
	return time(NULL);
}