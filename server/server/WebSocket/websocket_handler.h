#ifndef __WEBSOCKET_HANDLER__
#define __WEBSOCKET_HANDLER__

#include <iostream>
#include <map>
#include <sstream>
#include "base64.h"
#include "sha1.h"
#include "websocket_request.h"
#include"websocket_respond.h"
#include "../define.h"

#define MAGIC_KEY "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"

enum WEBSOCKET_STATUS {
	WEBSOCKET_UNCONNECT = 0,
	WEBSOCKET_HANDSHARKED = 1,
};

typedef std::map<std::string, std::string> HEADER_MAP;

class Websocket_Handler{
public:
	Websocket_Handler(int fd);
	~Websocket_Handler();
	int process(char* buffer);
	inline char *getbuff();
    void ProcessDataToWeb(DataBuffer* buffer, char* outptr, int& size);
    // 已经废弃 直接在process里面返回buffer
    void GetParseData(char* buffer);
private:
	int handshark();
	void parse_str(char *request);
	int fetch_http_info();
	int send_data(char *buff);
private:
	char buff_[MAX_NUM_WEB_ALL];
	WEBSOCKET_STATUS status_;
	HEADER_MAP header_map_;
	int fd_;
	Websocket_Request *request_;
    Websocket_Respond *respond_;
};

inline char *Websocket_Handler::getbuff() {
    return buff_;
}

#endif
