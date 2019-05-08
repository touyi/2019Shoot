#pragma once
#include"server.h"
class PartnerProcess
{
public:
    PartnerProcess();
    ~PartnerProcess();
    void SetScreen(CClient* client);

    void SetMobilde(CClient* client);
    CClient* GetScreen();
    CClient* GetMobile();

    void ExchangeData();

    bool ParseWebInfo(std::vector<DataBuffer>& parseBuffer);

    bool IsPair();
private:
    mutable std::mutex mut;
    CClient* Screen = NULL;
    CClient* Mobile = NULL;
    
};