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
        // �ݲ�֧����ô��������
        return WS_ERROR_FRAME;
    }


    uint8_t payloadFieldExtraBytes = (messageLength <= 0x7d) ? 0 : 2;


    // header: 2�ֽ�, maskλ����Ϊ0(������), ������masking key������д, ʡ��4�ֽ�
    uint8_t frameHeaderSize = 2 + payloadFieldExtraBytes;
    uint8_t *frameHeader = new uint8_t[frameHeaderSize];


    memset(frameHeader, 0, frameHeaderSize);


    // finλΪ1, ��չλΪ0, ����λΪframeType
    frameHeader[0] = static_cast<uint8_t>(0x80 | frameType);


    // ������ݳ���
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


    // �������
    uint32_t frameSize = frameHeaderSize + messageLength;

    memcpy(outFrame, frameHeader, frameHeaderSize);
    memcpy(outFrame + frameHeaderSize, inMessage.c_str(), messageLength);
    outFrame[frameSize] = '\0';
    size = frameSize;

    delete[] frameHeader;


    return ret;
}