cmake_minimum_required(VERSION 3.20)

project(device-sim
	VERSION 0.1.0
	LANGUAGES CXX
)

set(CMAKE_CXX_STANDARD 11)
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)
set(CMAKE_BUILD_TYPE DEBUG)

# find_package(OpenSSL)

add_library(sim STATIC
	simulator/sim_client.cpp
	src/device.cpp
	src/dataClient.cpp
	src/sensor.cpp
)
target_compile_definitions(sim PRIVATE SIMULATOR)
target_include_directories(sim PUBLIC simulator src src/lib)

add_executable(device-sim simulator/main.cpp)
target_compile_definitions(device-sim PRIVATE SIMULATOR)
target_link_libraries(device-sim PRIVATE sim)
# target_compile_definitions(device-sim PRIVATE NO_LTE)

add_executable(api-test simulator/api_test.cpp)
target_compile_definitions(api-test PRIVATE SIMULATOR)
target_link_libraries(api-test PRIVATE sim)