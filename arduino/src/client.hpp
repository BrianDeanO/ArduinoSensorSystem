#pragma once
#include "../config.hpp"

class DataClient {
public:
    virtual bool send(const char* command, uint32_t size);
    virtual uint64_t get_time();

    uint64_t fake_last_time = 0;
};

#ifndef NO_LTE
#include "time.hpp"
#include "lib/SparkFun_LTE_Shield.h"

class LTEClient : public DataClient {
public:
    LTEClient(LTE_Shield* lte);
    bool send(const char* command, uint32_t size) override;
    uint64_t get_time() override;

    LTE_Shield* lte;
    const char* addr;
    unsigned port;
};

#endif