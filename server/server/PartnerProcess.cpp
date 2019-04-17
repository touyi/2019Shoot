#include "PartnerProcess.h"
#include "protocol/Protocol.pb.h"
#include "protocolnumber.h"

PartnerProcess::PartnerProcess()
{
}


PartnerProcess::~PartnerProcess()
{
}

void PartnerProcess::SetScreen(CClient * client)
{
    std::lock_guard<std::mutex>lk(mut);
    Screen = client;
}

void PartnerProcess::SetMobilde(CClient * client)
{
    std::lock_guard<std::mutex>lk(mut);
    Mobile = client;
}

CClient * PartnerProcess::GetScreen()
{
    return Screen;
}

CClient * PartnerProcess::GetMobile()
{
    return Mobile;
}

void PartnerProcess::ExchangeData()
{
    if (!IsPair()) {
        return;
    }
    Message::KeyChange keychange;
    while (!Mobile->isEmpty()) {
        // TODO webÔÝÊ±²»ÓÃpb
        DataBuffer* buffer = Mobile->PopNextData();
        if (strcmp(buffer->buffer, "F#") == 0) {
            auto keydata = keychange.add_keydatas();
            keydata->set_key(0);
            keydata->set_keystate(1);
        }
    }
    char tempbuf[MAX_NUM_BUF];
    memset(tempbuf, 0, MAX_NUM_BUF);
    int size = keychange.ByteSize();
    if (size > 0) {
        keychange.SerializeToArray(tempbuf, size);
        Screen->SetFrameSend(ProtocolNumber::KEY_STATE, tempbuf, size);
    }
    
}

bool PartnerProcess::IsPair()
{
    return Mobile != NULL && Screen != NULL;
}
