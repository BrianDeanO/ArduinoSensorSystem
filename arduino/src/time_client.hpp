#pragma once

#include <bits/types/time_t.h>

#ifdef SIMULATOR
	#include <ctime>
#else
	#include <NTPClient.h>
	#include "sparkfun_udp.hpp"
#endif

// A wrapper around the NTPClient class, which gets the current time 
// from an NTP server, synchronizing periodically.
class TimeClient {
public:
	TimeClient() 
	#ifndef SIMULATOR
		: ntp(NTPClient(this->udp, "pool.ntp.org", 3600, 60000))
	#endif
	{
		#ifndef SIMULATOR
			ntp.begin();
		#endif
	}

	virtual time_t get_time() {
		#ifdef SIMULATOR
			return time(NULL);
		#else
			return ntp.getEpochTime();
		#endif
	}

	virtual void update() {
		#ifndef SIMULATOR
			ntp.update();
		#endif
	}

private:
	#ifndef SIMULATOR
		Sparkfun_UDP udp(LTE_SHIELD_HW_SERIAL);
		NTPClient ntp;
	#endif
};
