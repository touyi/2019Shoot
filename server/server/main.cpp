#include "server.h"

int main(int argc, char* argv[])
{
	//��ʼ��������
	if (!initSever())
	{
		exitServer();
		return SERVER_SETUP_FAIL;
	}

	//��������
	if (!startService())
	{
		exitServer();
		return SERVER_SETUP_FAIL;
	}

	//��������
	Run();

	//�˳����̣߳�������Դ
	exitServer();

	return 0;
}
