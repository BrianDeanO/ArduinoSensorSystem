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
	std::cout << "Enter an update interval in seconds > ";
	std::getline(std::cin, input);
	time_t update_interval = std::stoi(input);

	sensors.push_back(new ExampleSensor("sim_temp"));
	Device device(sensors.data(), sensors.size(), &client);
	device.set_record_interval(update_interval);

	std::cout << "Registering device..." << std::endl;
	device.init();

	std::cout << "Simulating device. Ctrl+C to exit." << std::endl;
	while(true) {
		std::cout << "Update event." << std::endl;
		device.update();
		time_t duration = (device.next_update() - client.get_time()) * 1000000;
		duration = duration > 0 ? duration : 0; // Ensure we don't sleep for a negative duration
		usleep(duration);
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