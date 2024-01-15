# Development
`arduino.ino` is the main sketch file. All c/cpp files within the `src` folder will be compiled and linked together upon compilation.

Install the [Arduino VSCode extension](https://marketplace.visualstudio.com/items?itemName=vsciot-vscode.vscode-arduino) or the [Arduino IDE](https://www.arduino.cc/en/software) to upload the sketch to the board.

# Communication Protocol

Devices communicate over TCP (by default port _) with packets using the following
format:

<details> <summary>Type legend</summary>
```
u16: Unsigned 16 bit integer
u32: Unsigned 32 bit integer
u64: Unsigned 64 bit integer
f64: Double width (64 bit) floating point
char: Signed 8 bit character
type[num]: An array of `type` of length `num`. Following pattern repeats `num` times
```
</details>


```
	u8: command
	char[32]: Device ID
	...
```

Commands are as follows: 
```
Register Device
	u8: command = 1
	char[32]: Device ID
	u16: Sensor count
	Sensor Definitions
		char[32]: Sensor ID
		char[32]: Unit description

Send Data Points
	u8: command = 2
	char[32]: Device ID
	u16: Sensor count
	Sensor Data[Sensor count]
		char[32]: Sensor ID
		u32: Num points
		Points[Num points]
			f64: Value
			u64: Time point was recorded
```

# Simulator
The simulator program acts as a fake device client, communicating with the server
without hardware.

### Building the Simulator
Requirements:
- OpenSSL 1.1.1 or 3.0
- CMake and a C++ compiler

```
cmake -B build -S source
cmake --build build
./build/device-sim
```