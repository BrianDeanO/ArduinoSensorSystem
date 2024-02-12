#include "dataClient.hpp"

#ifndef SIMULATOR
#define CONTENT_TYPE "application/json"

int handle_httpclient_response(HttpClient& http, const char* url, char* response, unsigned response_size)
{
	int statusCode = http.responseStatusCode();
	if(statusCode / 100 != 2) { // Check for 200 status code (Success)
		DEBUG("Status code %d returned from request %s\n", statusCode, url);
		return -statusCode;
	}

	if(!response) {
		// DEBUG("No response for %s\n", url);
		return 0;
	}

	String resp = http.responseBody();
	resp.toCharArray(response, response_size);
	return resp.length();
}

int SerialDataClient::get(const char* url, char* response, unsigned response_size)
{
	http.stop(); // Reset the connection, failing to do this results in empty requests being sent
	serial_client.flush_input();
	serial_client.print("\nREQUEST\n");
	http.get(url);
	serial_client.print("\nREQUEST END\n");
	return handle_httpclient_response(this->http, url, response, response_size);
}

int SerialDataClient::post(const char* url, const char* body, char* response, unsigned response_size)
{
	http.stop();
	serial_client.flush_input();
	serial_client.print("\nREQUEST\n");
	http.post(url, CONTENT_TYPE, body);
	serial_client.print("\nREQUEST END\n");
	return handle_httpclient_response(this->http, url, response, response_size);
}

int SerialDataClient::put(const char* url, const char* body, char* response, unsigned response_size)
{
	http.stop();
	serial_client.flush_input();
	serial_client.print("\nREQUEST\n");
	http.put(url, CONTENT_TYPE, body);
	serial_client.print("\nREQUEST END\n");
	return handle_httpclient_response(this->http, url, response, response_size);
}


int LTEDataClient::get(const char* url, char* response, unsigned response_size)
{
	http.stop();
	http.get(url);
	return handle_httpclient_response(this->http, url, response, response_size);
}

int LTEDataClient::post(const char* url, const char* body, char* response, unsigned response_size)
{
	http.stop();
	http.post(url, CONTENT_TYPE, body);
	return handle_httpclient_response(this->http, url, response, response_size);
}

int LTEDataClient::put(const char* url, const char* body, char* response, unsigned response_size)
{
	http.stop();
	http.put(url, CONTENT_TYPE, body);
	return handle_httpclient_response(this->http, url, response, response_size);
}

uint64_t LTEDataClient::get_time() {
	tmElements_t tm;
	uint8_t tz;
	LTE_Shield_error_t err = lte->clock(&tm.Year, &tm.Month, &tm.Day, &tm.Hour, &tm.Minute, &tm.Second, &tz);
	if(err != LTE_SHIELD_ERROR_SUCCESS) {
		DEBUG("Error getting lte time: lte err code %d\n", err);
		return 0;
	}
	else {
		return makeTime(tm);
	}
}

#endif // SIMULATOR