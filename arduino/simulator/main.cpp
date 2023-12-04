#include <iostream>

#include "sim_http_client.hpp"
#include "time_client.hpp"
#include "device.hpp"

#include "drivers/example_driver.hpp"

void print_menu() {
	std::cout << "1) Start simulated device\n"
	        //   << "2) Send register command\n"
	        //   << "3) Send datapoint command\n"
	        //   << "3) Get device configuration\n"
			  << "\n"
	          << "0) Exit menu\n"
			  << std::endl;
}

void run_sim_device(std::string addr, unsigned port) {
	TimeClient time;
	HttpSim http(addr.c_str(), port);

	std::vector<Sensor> sensors;
	std::string id, input;

	std::cout << "Enter a device ID > " << std::endl;
	std::getline(std::cin, id);
	std::cout << "Enter an update interval in seconds > " << std::endl;
	std::getline(std::cin, input);
	time_t update_interval = std::stoi(input);

	ExampleDriver driver("temp", "C");
	sensors.push_back(Sensor("Temperature 1", &driver));
	Device device(id.c_str(), sensors.data(), sensors.size(), &http, &time);
	device.set_record_interval(update_interval);

	std::cout << "Registering device..." << std::endl;
	device.register_device();

	std::cout << "Simulating device. Ctrl+C to exit." << std::endl;
	while(true) {
		std::cout << "Update event." << std::endl;
		device.update();
		sleep(device.next_update() - time.get_time());
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