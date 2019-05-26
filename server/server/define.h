#pragma once
#define TIMEFOR_THREAD_CLIENT		500		//�߳�˯��ʱ��

#define	MAX_NUM_CLIENT		10				//���ܵĿͻ��������������
#define	MAX_NUM_BUF			60				//����������󳤶�
#define HEAD_SIZE           sizeof(ProtoHead)
#define	MAX_NUM_DATA		(MAX_NUM_BUF + HEAD_SIZE)	//���ݰ�����󳤶�
#define MAX_NUM_WEB_HEAD    1024
#define MAX_NUM_WEB_ALL    (MAX_NUM_WEB_HEAD + MAX_NUM_DATA)
#define INVALID_OPERATOR	1				//��Ч�Ĳ�����
#define INVALID_NUM			2				//��ĸΪ��
#define ZERO				0				//��
#define FRAME_TIME           67
const int MobileCMDProtocol = 100;

typedef int ClientID;
typedef unsigned short UShort;

#include<memory.h>

//���ݰ�ͷ�ṹ���ýṹ��win32��Ϊ4byte
struct ProtoHead
{
    UShort	proto;	//����
    UShort	Length;	//���ݰ��ĳ���(����ͷ�ĳ���)
};

enum SocketConnType {
    Unknow,
    Normal,
    Web,
};

union DataBuffer {
    struct DataPackage {
        ProtoHead head;
        char datas[MAX_NUM_BUF];
    } Package;
    char buffer[MAX_NUM_DATA];
public:
    DataBuffer() = default;
    DataBuffer(char* _buffer) {
        memcpy(this->buffer, _buffer, MAX_NUM_DATA);
    }
    DataBuffer(const DataBuffer& buffer) {
        (*this) = buffer;
    }
    DataBuffer(const DataBuffer*&buffer) {
        (*this) = *buffer;
    }
    const DataBuffer& operator=(const DataBuffer& _buffer) {
        memcpy(this->buffer, _buffer.buffer, MAX_NUM_DATA);
        return (*this);
    }
};

//���ݰ��е����ݽṹ
typedef struct _data
{
    char	buf[MAX_NUM_BUF];//����
}DATABUF, *pDataBuf;