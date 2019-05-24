#ifndef __WEBSOCKET_RESPOND__
#define __WEBSOCKET_RESPOND__
#include<string>
enum WS_FrameType
{
    WS_EMPTY_FRAME = 0xF0,
    WS_ERROR_FRAME = 0xF1,


    WS_TEXT_FRAME = 0x01,
    WS_BINARY_FRAME = 0x02,


    WS_PING_FRAME = 0x09,
    WS_PONG_FRAME = 0x0A,


    WS_OPENING_FRAME = 0xF3,


    WS_CLOSING_FRAME = 0x08


};

class Websocket_Respond {
public:
	Websocket_Respond();
	~Websocket_Respond();
    int wsEncodeFrame(std::string inMessage, char* outFrame, enum WS_FrameType frameType, int& size);
    
};



#endif
