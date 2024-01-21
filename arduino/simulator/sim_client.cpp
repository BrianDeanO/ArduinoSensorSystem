#include "sim_client.hpp"
#include <time.h>
#include <stdio.h>
#include <unistd.h>
#include <sys/socket.h>
#include <netinet/ip.h>
#include <arpa/inet.h>

bool SimClient::send(const char* command, uint32_t size) {
	struct sockaddr_in serv_addr;
	int status, client_fd;
	if ((client_fd = socket(AF_INET, SOCK_STREAM, 0)) < 0) {
		printf("\nSocket creation error\n");
		return false;
	}

	serv_addr.sin_family = AF_INET;
	serv_addr.sin_port = htons(this->port);
	if(inet_pton(AF_INET, this->addr, &serv_addr.sin_addr) <= 0) {
		printf("\nInvalid address\n");
		return false;
	}

	status = connect(client_fd, (struct sockaddr*)&serv_addr, sizeof(serv_addr));
	if (status < 0) {
		printf("\nConnection failed\n");
		return false;
	}

	::send(client_fd, (void*)command, (size_t)size, 0);

	// Read verification response, must match "ok"
	char response[16];
	read(client_fd, response, 15);
	if(response[0] == 'o' && response[1] == 'k') {
		close(client_fd);
		return true;
	}
	else {
		close(client_fd);
		return false;
	}
}

uint64_t SimClient::get_time() {
	return time(NULL);
}