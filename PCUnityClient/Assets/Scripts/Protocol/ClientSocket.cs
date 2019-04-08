﻿using System;
using System.Text;
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
            byte[] bytes = Encoding.ASCII.GetBytes(item.buffer);
            
        }
    }
}