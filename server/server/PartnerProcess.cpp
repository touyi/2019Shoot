#include "PartnerProcess.h"
#include "protocol/Protocol.pb.h"
#include "protocolnumber.h"
#include<map>
#include<sstream>

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

void PartnerProcess::ParseVec3(const char * obj, float & x, float & y, float & z)
{
    using namespace std;
    const char* src = strstr(obj, "O#");
    if (src == NULL) {
        return;
    }
    src += 2;
    stringstream stream(src);
    stream >> x >> y;
    std::cout << x << " " << y << std::endl;
    z = 0;
}

bool PartnerProcess::ParsePCInfo(std::vector<DataBuffer>& parseBuffer)
{
    using namespace Message;
    CommandList cmdList;
    bool isEnd = false;
    while (!Screen->isEmpty()) {
        DataBuffer* buffer = Screen->PopNextData();
        if (!cmdList.ParseFromArray(buffer->Package.datas, buffer->Package.head.Length - HEAD_SIZE)) {
            LogManager::Error("ParsePCInfo Error");
            continue;
        }
        int size = cmdList.commanddatas_size();
        for (int i = 0; i < size; i++) {
            const Command& cmd = cmdList.commanddatas(i);
            if (cmd.ctype() == CmdType::GameEnd) {
                isEnd = true;
                break;
            }
        }
    }
    if (isEnd) {
        DataBuffer buffer;
        memset(&buffer, 0, sizeof(buffer));
        const char* endStr = "E#";
        buffer.Package.head.proto = 10;
        buffer.Package.head.Length = HEAD_SIZE + strlen(endStr);
        memcpy(buffer.Package.datas, endStr, strlen(endStr));
        parseBuffer.push_back(buffer);
        return true;
    }
    else {
        return false;
    }
}

bool PartnerProcess::ParseWebInfo(std::vector<DataBuffer>& parseBufferVec)
{
    using namespace Message;
    KeyChange keychange;
    CommandList cmdList;
    VecList vecList;
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
        if (strstr(buffer->buffer, "O#") != NULL) {
            Vec3* vec = vecList.add_vec();
            float x, y, z;
            this->ParseVec3(buffer->buffer, x, y, z);
            vec->set_x(x);
            vec->set_y(y);
            vec->set_z(z);
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
    int vecSize = vecList.ByteSize();
    if (vecSize > 0) {
        DataBuffer parseBuffer;
        parseBuffer.Package.head.proto = 3;
        parseBuffer.Package.head.Length = vecSize + HEAD_SIZE;
        vecList.SerializeToArray(parseBuffer.Package.datas, vecSize);
        parseBufferVec.push_back(parseBuffer);
    }
    if (cmdSize <= 0 && keyChangeSize <= 0 && vecSize <= 0) {
        return false;
    }
    
    return true;
}

void PartnerProcess::ExchangeData()
{
    if (!IsPair()) {
        return;
    }
    std::vector<DataBuffer> *parseBuffer = new std::vector<DataBuffer>();
    if (this->ParseWebInfo(*parseBuffer)) {
        for (int i = 0; i < parseBuffer->size(); i++) {
            Screen->SetFrameSend((*parseBuffer)[i]);
        }
    }

    parseBuffer->clear();
    if (this->ParsePCInfo(*parseBuffer)) {
        for (int i = 0; i < parseBuffer->size(); i++) {
            Mobile->SetFrameSend((*parseBuffer)[i]);
        }
    }
    delete parseBuffer;

    
}

bool PartnerProcess::IsPair()
{
    return Mobile != NULL && Screen != NULL;
}

