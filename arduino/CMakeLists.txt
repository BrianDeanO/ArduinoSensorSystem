cmake_minimum_required(VERSION 3.20)

project(device-sim
	VERSION 0.1.0
	LANGUAGES CXX
)

set(CMAKE_CXX_STANDARD 11)
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)

find_package(OpenSSL)

add_executable(device-sim
	simulator/main.cpp
	simulator/sim_client.cpp
	src/device.cpp
	src/client.cpp
)
target_include_directories(device-sim PRIVATE simulator src src/lib)
target_compile_definitions(device-sim PRIVATE SIMULATOR)
target_compile_definitions(device-sim PRIVATE NO_LTE)