#!/usr/bin/env python

import socket
import serial
import sys
import textwrap
from enum import Enum
from datetime import datetime

if len(sys.argv) != 3:
	print("Usage: " + sys.argv[0] + " <address> <port>")
	sys.exit(1)

# Address and port from args
addr = sys.argv[1]
port = int(sys.argv[2])


class State(Enum):
	READY = 0
	IN_REQUEST = 1
	IN_RESPONSE = 2

def process_lines(ser, state: State):
	request_bytes = b''
	response_bytes = b''
	while True:
		line = ser.readline()
		if line == b'REQUEST_TIME\n':
			# Sent the current time since epoch to the device so it has a timestamp it can
			# use to send points without having duplicate keys
			now = (datetime.now() - datetime(1970, 1, 1)).total_seconds()
			now_bytes = (str(int(now)) + '\n').encode("utf-8")
			ser.write(now_bytes)
			print("Sending timestamp")
			continue

		if state == State.IN_REQUEST:
			if line == b'REQUEST END\n':
				state = State.IN_RESPONSE
				line = b'MORE PLS\n'

				print("Sending request:\n" + textwrap.indent(request_bytes.decode("utf-8"), "    "))	
				response_bytes = send_request(request_bytes)
				print("Got response:\n" + textwrap.indent(response_bytes.decode("utf-8"), "    "))
				request_bytes = b''

			else:
				request_bytes += line
				continue

		if state == State.IN_RESPONSE:
			if line == b'MORE PLS\n':
				if len(response_bytes) > 0:
					end = min(64, len(response_bytes))
					ser.write(response_bytes[:end])
					print("Wrote " + str(end) + " bytes")
					response_bytes = response_bytes[end:]
				else:
					print("Response done")
					# ser.write([ 0x0 ])
					state = State.READY
				continue
			
		if line == b'REQUEST\n':
			print("Request start")
			state = State.IN_REQUEST
			request_bytes = b''
			continue

		print(line)

def send_request(request_bytes: bytes):
	# Create a socket and connect to the server
	sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
	sock.connect((addr, port))
	sock.sendall(request_bytes)

	# Get the response from the server
	response = b''
	while True:
		r = sock.recv(1024)
		if not r:
			sock.close()
			return response
		response += r


ser = serial.Serial('/dev/ttyACM0', 9600)
# ser = serial.Serial('/tmp/ttyV1', 9600)
ser.reset_input_buffer()
ser.reset_output_buffer()
state = State.READY

process_lines(ser, state)
