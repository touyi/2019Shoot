#pragma once
#include<string>
class WebSocketHelper
{
private:
    WebSocketHelper() = default;
    ~WebSocketHelper() = default;

public:
    int HandShake(std::string &request, std::string &response);
};

