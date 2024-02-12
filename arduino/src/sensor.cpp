#include "../config.hpp"
#include "sensor.hpp"
#include "lib/ArduinoJson.h"

void Sensor::cache_data_point(uint64_t time) {
	for(unsigned ch = 0; ch < channel_count(); ch++) {
		double val;
		if(acquire_channel_value(ch, val)) {
			Datapoint& p = _cache[_cache_index];
			_cache_index = (_cache_index + 1) % DATAPOINT_CACHE_SIZE;
			p.value = val;
			p.time = time;
			p.channel = ch;
		}
	}
}

bool Sensor::get_last_point(Datapoint& point) {
	if(_cache_index == 0) {
		// No points to report
		return false;
	}
	point = _cache[_cache_index - 1];
	return true;
}

void Sensor::pop_last_point() {
	if(_cache_index > 0)
		_cache_index--;
}

void Sensor::reset() {
	_cache_index = 0;
}

bool Sensor::register_sensor(DataClient* client, char* buf, uint16_t buf_size, uint32_t deviceID) 
{
	char url_buf[100];
	sprintf(url_buf, "/api/Sensor/ident/%s", this->ident());

	int result = client->get(url_buf, buf, RESPONSE_BUFFER_SIZE);
	if(result > 0) {
		JsonDocument j;
		deserializeJson(j, buf);
		if(!j.containsKey("sensorID")) {
			DEBUG("Sensor registration response contained no id!\n");
			return false;
		}

		this->_id = j["sensorID"].as<uint32_t>();
		return true;
	}
	// Check for 400 level response codes, indicating the identifier does not 
	// yet exist
	else if(result / -100 == 4) {
		// Register a new device with our defaults
		JsonDocument j;
		j["sensorIdent"] = this->ident();
		j["sensorName"] = this->ident();
		j["sensorType"] = this->sensor_type();
		j["channelCount"] = this->channel_count();
		j["deviceID"] = deviceID;
		serializeJson(j, buf, buf_size);

		result = client->post("/api/Sensor", buf, buf, RESPONSE_BUFFER_SIZE);
		if(result > 0) {
			j.clear();
			deserializeJson(j, buf);
			if(!j.containsKey("sensorID")) {
				DEBUG("New sensor registration response contained no id!\n");
				return false;
			}

			this->_id = j["sensorID"].as<uint32_t>();
			return true;
		}
		else {
			DEBUG("Failed to register new sensor: %d", result);
		}
	}

	return false;
}