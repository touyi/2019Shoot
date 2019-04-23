#include "PartnerProcess.h"
#include "protocol/Protocol.pb.h"
#include "protocolnumber.h"
#include<map>

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

bool PartnerProcess::ParseWebInfo(DataBuffer& parseBuffer)
{
    using namespace Message;
    KeyChange keychange;
    using std::map;
    using std::pair;
    map<KeyType, KeyState> keyMap;
    while (!Mobile->isEmpty()) {
        // TODO webÔÝÊ±²»ÓÃpb
        DataBuffer* buffer = Mobile->PopNextData();
        if (strcmp(buffer->buffer, "F#") == 0) {
            keyMap[KeyType::Fire] = KeyState::Click;
        }
        if (strcmp(buffer->buffer, "C#") == 0) {
            keyMap[KeyType::Change] = KeyState::Click;
        }
        delete buffer;
    }
    if (keyMap.size() <= 0) {
        return false;
    }
    for (map<KeyType, KeyState>::iterator iter = keyMap.begin(); iter != keyMap.end(); iter++) {
        KeyData* data = keychange.add_keydatas();
        data->set_key(iter->first);
        data->set_keystate(iter->second);
    }
    int size = keychange.ByteSize();
    if (size <= 0) {
        return false;
    }
    
    parseBuffer.Package.head.proto = 1;
    parseBuffer.Package.head.Length = size + sizeof(parseBuffer.Package.head);
    keychange.SerializeToArray(parseBuffer.Package.datas, size);
    return true;
}

void PartnerProcess::ExchangeData()
{
    if (!IsPair()) {
        return;
    }
    DataBuffer parseBuffer;
    if (this->ParseWebInfo(parseBuffer)) {
        Screen->SerFrameSend(parseBuffer);
    }
    
}

bool PartnerProcess::IsPair()
{
    return Mobile != NULL && Screen != NULL;
}
