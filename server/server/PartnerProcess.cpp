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

CClient * PartnerProcess::SetScreen(CClient * client)
{
    std::lock_guard<std::mutex>lk(mut);
    auto cl = Screen;
    Screen = client;
    return cl;

}

CClient * PartnerProcess::SetMobilde(CClient * client)
{
    std::lock_guard<std::mutex>lk(mut);
    auto cl = Mobile;
    Mobile = client;
    return cl;
}

CClient * PartnerProcess::GetScreen()
{
    return Screen;
}

CClient * PartnerProcess::GetMobile()
{
    return Mobile;
}

bool PartnerProcess::ParseWebInfo(std::vector<DataBuffer>& parseBufferVec)
{
    using namespace Message;
    KeyChange keychange;
    CommandList cmdList;
    using std::map;
    using std::pair;
    map<KeyType, KeyState> keyMap;
    while (!Mobile->isEmpty()) {
        // TODO 这里的解析可以再优化一哈
        DataBuffer* buffer = Mobile->PopNextData();
        if (strcmp(buffer->buffer, "F1#") == 0) {
            keyMap[KeyType::Fire] = KeyState::Down;
        }
        if (strcmp(buffer->buffer, "F0#") == 0) {
            if (keyMap.find(KeyType::Fire) != keyMap.end()) {
                keyMap[KeyType::Fire] = KeyState::Click;
            }
            else {
                keyMap[KeyType::Fire] = KeyState::Up;
            }
                
        }
        if (strcmp(buffer->buffer, "C1#") == 0) {
            keyMap[KeyType::Change] = KeyState::Down;
        }
        if (strcmp(buffer->buffer, "C0#") == 0) {
            if (keyMap.find(KeyType::Change) != keyMap.end()) {
                keyMap[KeyType::Change] = KeyState::Click;
            }
            else {
                keyMap[KeyType::Change] = KeyState::Up;
            }
        }
        // CMD
        if (strcmp(buffer->buffer, "E#") == 0) {
            Command* cmd = cmdList.add_commanddatas();
            cmd->set_ctype(CmdType::GameEnd);
            
        }
        delete buffer;
    }
    
    for (map<KeyType, KeyState>::iterator iter = keyMap.begin(); iter != keyMap.end(); iter++) {
        KeyData* data = keychange.add_keydatas();
        data->set_key(iter->first);
        data->set_keystate(iter->second);
    }
    parseBufferVec.clear();
    int keyChangeSize = keychange.ByteSize();
    if (keyChangeSize > 0) {
        DataBuffer parseBuffer;
        parseBuffer.Package.head.proto = 1;
        parseBuffer.Package.head.Length = keyChangeSize + sizeof(parseBuffer.Package.head);
        keychange.SerializeToArray(parseBuffer.Package.datas, keyChangeSize);
        parseBufferVec.push_back(parseBuffer);
    }
    int cmdSize = cmdList.ByteSize();
    if (cmdSize > 0) {
        DataBuffer parseBuffer;
        parseBuffer.Package.head.proto = 2;
        parseBuffer.Package.head.Length = cmdSize + HEAD_SIZE;
        cmdList.SerializeToArray(parseBuffer.Package.datas, cmdSize);
        parseBufferVec.push_back(parseBuffer);
    }
    if (cmdSize <= 0 && keyChangeSize <= 0) {
        return false;
    }
    
    return true;
}

void PartnerProcess::ExchangeData()
{
    if (!IsPair()) {
        return;
    }
    std::vector<DataBuffer> parseBuffer;
    if (this->ParseWebInfo(parseBuffer)) {
        for (int i = 0; i < parseBuffer.size(); i++) {
            Screen->SetFrameSend(parseBuffer[i]);
        }
    }
    
}

bool PartnerProcess::IsPair()
{
    return Mobile != NULL && Screen != NULL;
}

