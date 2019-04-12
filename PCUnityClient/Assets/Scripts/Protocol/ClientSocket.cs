using System;
using System.IO;
using System.Text;
using Message;
using UnityEngine;
using Wrapper;

namespace Protocol
{
    public class ClientSocket : Singleton<ClientSocket>
    {
        private ClientWarp _warp = null;
        private bool isConnect = false;

        public void Init()
        {
            _warp = new ClientWarp();
            _warp.InitClient("127.0.0.1", 6666);
            if (_warp.ConnectServer() != 1)
            {
                Debug.LogError("网络连接初始化失败");
                return;
            }

            isConnect = true;
        }

        public void Uninit()
        {
            _warp.ExitClient();
        }
        public void Update(float deltaTime)
        {
            if (!this._warp.IsConnected())
            {
                if (isConnect)
                {
                    isConnect = false;
                    //  TODO 异步尝试重新连接
                }
                Debug.Log("尝试连接 ");
                return;
            }

            while (!this._warp.IsDataEmpty())
            {
                DataItem item = this._warp.PopNextData();
                this.HandleData(item);
            }
        }

        private byte[] memBytes = new byte[64];
        private void HandleData(DataItem item)
        {
            Debug.Log(string.Format("协议：{0} 内容：{1}",item.protocol,item.buffer));
            // TODO 重用stream 防止GC
            using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(item.buffer)))
            {
                try
                {
                    KeyChange change = ProtoBuf.Serializer.Deserialize<KeyChange>(stream);
                    Debug.Log(string.Format("key:{0}, keyState:{1}", change.keyDatas[0].key,
                        change.keyDatas[1].keyState));
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
            }
            

        }
    }
}