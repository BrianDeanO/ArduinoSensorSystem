# Development
`arduino.ino` is the main sketch file. All c/cpp files within the `src` folder will be compiled and linked together upon compilation.

Install the [Arduino VSCode extension](https://marketplace.visualstudio.com/items?itemName=vsciot-vscode.vscode-arduino) or the [Arduino IDE](https://www.arduino.cc/en/software) to upload the sketch to the board.

# Simulator
The simulator program acts as a fake device client, communicating with the server
without hardware.

### Building the Simulator
Requirements:
- CMake and a C++ compiler

```
cmake -B build -S source
cmake --build build
./build/device-sim
```

# Serial-TCP Bridge
`serial_tcp_bridge.py` is a python script allowing the device to use a serial
port to send http requests to the server. Upload the sketch to the device with
`NO_LTE` defined, and run the script. You may need to adjust the serial port it
connects to.

Requires pySerial:
```
pip3 install pySerial
./serial_tcp_bridge.py <addr> <port>
```