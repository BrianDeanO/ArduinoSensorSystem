#pragma once
#include "../config.hpp"
#include "lib/SparkFun_LTE_Shield.h"

class DataClient {
public:
    virtual bool send(const char* command);
    virtual uint64_t get_time();
};

#ifndef NO_LTE
#include "time.hpp"

class LTEClient : public DataClient {
public:
    LTEClient(LTE_Shield* lte);
    bool send(const char* command) override;
    uint64_t get_time() override;

    LTE_Shield* lte;
};

#endif