#pragma once
#include"server.h"
class PartnerProcess
{
public:
    PartnerProcess();
    ~PartnerProcess();
    CClient * SetScreen(CClient* client);

    CClient * SetMobilde(CClient* client);
    CClient* GetScreen();
    CClient* GetMobile();

    void ExchangeData();

    bool ParseWebInfo(std::vector<DataBuffer>& parseBuffer);
    bool ParsePCInfo(std::vector<DataBuffer>& parseBuffer);

    bool IsPair();
private:
    void ParseVec3(const char* obj, float&x, float&y, float&z);
    mutable std::mutex mut;
    CClient* Screen = NULL;
    CClient* Mobile = NULL;
    
};