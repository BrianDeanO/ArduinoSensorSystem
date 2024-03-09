#include <iostream>

#include "../config.hpp"
#include "sim_client.hpp"
#include "device.hpp"
#include "drivers/example_driver.hpp"

#include <unistd.h>
#include <vector>

// Used by DEBUG macros defined in config.hpp
#if DEBUG_MODE == 1
char _dbg_msg[256];
#endif

void print_menu() {
	std::cout << "1) Start simulated device\n"
	        //   << "2) Send register command\n"
	        //   << "3) Send datapoint command\n"
	        //   << "3) Get device configuration\n"
			  << "\n"
	          << "0) Exit menu" << std::endl;
}

void run_sim_device(std::string addr, unsigned port) {
	SimClient client(addr.c_str(), port);

	std::vector<Sensor*> sensors;
	std::string input;

	sensors.push_back(new ExampleSensor("sim_temp"));
	Device device(sensors.data(), sensors.size(), &client);

	std::cout << "Registering device..." << std::endl;
	device.init();

	std::cout << "Simulating device. Ctrl+C to exit." << std::endl;
	while(true) {
		time_t current_time = client.get_time();

		device.poke_device(); // Send a "poke" to the server to let it know we're still alive
		device.get_config(); // Update config in case settings have changed since our last poll
		device.update(current_time); // Will send data to the server if current_time is past the next update time

		// Wait a maximum of CONFIG_POLL_INTERVAL ms
		time_t duration = (device.next_update() - current_time);
		if(duration > CONFIG_POLL_INTERVAL || duration < 0)
			duration = CONFIG_POLL_INTERVAL;
		std::cout << "Sleeping for " << duration << " seconds" << std::endl;
		usleep(duration * 1000000);
	}
}

int main(int argc, char** argv) {
	if(argc < 2 || argc > 3) {
		std::cout << "Usage: " << argv[0] << " <address> [port (80)]" << std::endl;
		return 1;
	}

	std::string addr = argv[1];
	unsigned port = 80;
	if (argc == 3) {
		port = std::stoi(argv[2]);
	}

	std::string input;
	while(true) {
		print_menu();
		std::cout << "> ";
		std::getline(std::cin, input);

		int option = -1;
		try {
			option = std::stoi(input);
		} catch(...) {
			continue;
		}
		if(option < 0 || option > 2) {
			std::cout << "Invalid option." << std::endl;
			continue;
		}

		switch(option) {
		case 0:
			return 0;

		case 1:
			run_sim_device(addr, port);
			break;
		}

	}
}