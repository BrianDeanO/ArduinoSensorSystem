#pragma once
#include "../config.hpp"
#include "lib/SparkFun_LTE_Shield.h"

class DataClient {
public:
    virtual bool send(const char* command, uint32_t size);
    virtual uint64_t get_time();

#ifdef NO_LTE
    uint64_t fake_last_time = 0;
#endif
};

#ifndef NO_LTE
#include "time.hpp"

class LTEClient : public DataClient {
public:
    LTEClient(LTE_Shield* lte);
    bool send(const char* command, uint32_t size) override;
    uint64_t get_time() override;

    LTE_Shield* lte;
};

#endif