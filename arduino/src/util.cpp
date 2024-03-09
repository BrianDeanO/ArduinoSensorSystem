#include "util.hpp"

uint64_t strtoull(char* str, char** endptr = nullptr) {
	uint64_t result = 0;
	while(*str) {
		if(*str < '0' || *str > '9') {
			if(endptr)
				endptr = &str;
			return result;
		}
		
		result *= 10;
		result += *str - '0';
		str++;
	}
	return result;
}