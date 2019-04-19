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

DataBuffer PartnerProcess::ParseWebInfo()
{

    Message::KeyChange keychange;
    while (!Mobile->isEmpty()) {
        // TODO web暂时不用pb
        DataBuffer* buffer = Mobile->PopNextData();
        if (strcmp(buffer->buffer, "F#") == 0) {
            auto keydata = keychange.add_keydatas();
            keydata->set_key(0);
            keydata->set_keystate(1);
        }
    }

    // NEXT TODO 解析为最终数据包
    return DataBuffer();
}

void PartnerProcess::ExchangeData()
{
    if (!IsPair()) {
        return;
    }
    DataBuffer parseBuffer = this->ParseWebInfo();
    Screen->SerFrameSend(parseBuffer);
}

bool PartnerProcess::IsPair()
{
    return Mobile != NULL && Screen != NULL;
}
