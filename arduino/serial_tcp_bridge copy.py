#!/usr/bin/env python

import socket
import serial
import sys
import textwrap

if len(sys.argv) != 3:
	print("Usage: " + sys.argv[0] + " <address> <port>")
	sys.exit(1)

# Address and port from args
addr = sys.argv[1]
port = int(sys.argv[2])

# Set up the serial port
ser = serial.Serial('/dev/ttyACM0', 9600)

while True:
	# Read the data from the serial port
	line = ser.readline()
	if line == b'REQUEST\n':
		request_bytes = b''
		print("Request start")
		while True:
			line_bytes = ser.readline()
			if line_bytes == b'REQUEST END\n':
				break
			request_bytes += line_bytes

		print("Sending request:\n" + textwrap.indent(request_bytes.decode("utf-8"), "    "))
		# Create a socket and connect to the server
		# print("Sending request:\n" + request_bytes.decode("utf-8"))
		sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
		sock.connect((addr, port))
		sock.sendall(request_bytes)

		# Get the response from the server
		response = sock.recv(1024)
		response_str = response.decode("utf-8")

		if False:#"Transfer-Encoding: chunked" in response_str:
			ser.write(response)
			# For each chunk received, check if the chunk size is 0
			# and send the whole chunk to the Arduino
			chunk_start = response_str.find("\r\n\r\n") + 4
			print("Got headers:\n" + textwrap.indent(response_str[:chunk_start], "    "))
			while True:
				chunk_size_end = response_str.find("\r\n", chunk_start)
				chunk_size = int(response_str[chunk_start:chunk_size_end], 16)
				chunk_end = chunk_size_end + 2 + chunk_size + 2
				chunk = response_str[chunk_start:chunk_end]
				chunk_start = chunk_end

				print("Got chunk:\n'''" + textwrap.indent(chunk, "    ") + "'''")

				if chunk_size == 0:
					print("Done")
					break
				elif chunk_start >= len(response_str):
					response = sock.recv(1024)
					if not response:
						break

					chunk_start = 0
					response_str = response.decode("utf-8")
					ser.write(response)

			ser.write(128)
		else:
			# Send the response to the Arduino
			print("Got response:\n" + textwrap.indent(response_str, "    "))
			ser.write(response)
			ser.write(255)

		print("Socket closed")
		sock.close()

	else:
		print(line);