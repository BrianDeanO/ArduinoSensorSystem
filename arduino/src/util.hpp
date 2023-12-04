#pragma once

#include <cstdarg>
#include <cstdio>
#include <cstring>

#define TRY_APPEND(cstr, ...) \
	if (!cstr.append(__VA_ARGS__)) { \
		return false; \
	}

// A wrapper around a char buffer, enabling safer appends without hiding allocations
class CString {
public:
	CString(char* str, int max_size) {
		this->_start = str;
		this->_end = _start + strlen(str);
		this->_max_size = max_size;
	}

	bool append(const char* str, int offset = 0) {
		unsigned str_len = strlen(str);
		if (len() + str_len > _max_size) {
			return false; // Failed to append, result would be too large
		}
		strcpy(_end + offset, str);
		_end += str_len;
		return true;
	}

	void clear() {
		_end = _start;
		*_start = '\0';
	}

	int len() {
		return _end - _start;
	}

	int capacity() {
		return _max_size;
	}

	char* c_str() {
		return _start;
	}

private:
	char* _start;
	char* _end;
	int _max_size;
};
