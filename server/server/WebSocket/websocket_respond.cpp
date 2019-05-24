#include "websocket_respond.h"
#include <windows.h>
Websocket_Respond::Websocket_Respond() { }

Websocket_Respond::~Websocket_Respond() { }

int Websocket_Respond::wsEncodeFrame(std::string inMessage, char* outFrame, enum WS_FrameType frameType, int& size)
{
    int ret = WS_EMPTY_FRAME;
    const uint32_t messageLength = inMessage.size();


    if (messageLength > 32767)
    {
        // 暂不支持这么长的数据
        return WS_ERROR_FRAME;
    }


    uint8_t payloadFieldExtraBytes = (messageLength <= 0x7d) ? 0 : 2;


    // header: 2字节, mask位设置为0(不加密), 则后面的masking key无须填写, 省略4字节
    uint8_t frameHeaderSize = 2 + payloadFieldExtraBytes;
    uint8_t *frameHeader = new uint8_t[frameHeaderSize];


    memset(frameHeader, 0, frameHeaderSize);


    // fin位为1, 扩展位为0, 操作位为frameType
    frameHeader[0] = static_cast<uint8_t>(0x80 | frameType);


    // 填充数据长度
    if (messageLength <= 0x7d)
    {
        frameHeader[1] = static_cast<uint8_t>(messageLength);
    }
    else
    {
        frameHeader[1] = 0x7e;


        uint16_t len = htons(messageLength);


        memcpy(&frameHeader[2], &len, payloadFieldExtraBytes);
    }


    // 填充数据
    uint32_t frameSize = frameHeaderSize + messageLength;

    memcpy(outFrame, frameHeader, frameHeaderSize);
    memcpy(outFrame + frameHeaderSize, inMessage.c_str(), messageLength);
    outFrame[frameSize] = '\0';
    size = frameSize;

    delete[] frameHeader;


    return ret;
}