#pragma once

// void strncpy(char* dest, const char* src, unsigned size) {
// 	for(unsigned i = 0; i < size; i++) {
// 		if(src[i] == '\0') {
// 			dest[i] = '\0';
// 			return;
// 		}
// 		dest[i] = src;
// 	}
// 	dest[size - 1] = '\0';
// }

struct SizedBuf {
	char* buffer;
	uint32_t len;
	uint32_t max_size;

	SizedBuf(char* array, uint32_t max_size) : buffer(array), max_size(max_size) {}
	bool has_capacity(uint32_t size) {
		return (this->len + size + 1 > max_size); // +1 to account for a null terminator
	}

	uint32_t remaining() { return this->max_size - this->len; }

	void append(void* src, uint32_t size) {
		if(!has_capacity(size)) {
			return;
		}

		memcpy(this->buffer + len, src, size);
		len += size;
		*(this->buffer + len) = 0; // Null terminate
	}
};