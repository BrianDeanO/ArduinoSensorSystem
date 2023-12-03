#pragma once

#include <bits/types/time_t.h>

#ifdef SIMULATOR
	#include <ctime>
#else
	#include <NTPClient.h>
#endif

// A wrapper around the NTPClient class, which gets the current time 
// from an NTP server, synchronizing periodically.
class TimeClient {
public:
	TimeClient() {
		#ifndef SIMULATOR
			ntp = new NTPClient(, "pool.ntp.org", 3600, 60000);
			ntp->begin();
		#endif
	}

	virtual time_t get_time() {
		#ifdef SIMULATOR
			return time(NULL);
		#else
			return ntp->getEpochTime();
		#endif
	}

	virtual void update() {
		#ifndef SIMULATOR
			ntp->update();
		#endif
	}

private:
	#ifndef SIMULATOR
		NTPClient* ntp;
	#endif
};
