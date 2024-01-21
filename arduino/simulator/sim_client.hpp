#include "client.hpp"

class SimClient : public DataClient {
public:
	SimClient(const char* addr, unsigned port) 
		: addr(addr), port(port) {}
    virtual bool send(const char* command, uint32_t size) override;
    virtual uint64_t get_time() override;

    const char* addr;
    unsigned port;
};