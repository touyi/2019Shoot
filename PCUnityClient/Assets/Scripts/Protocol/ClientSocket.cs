using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Message;
using NetInput;
using UnityEngine;
using Wrapper;

namespace Protocol
{
public unsafe class ClientSocket : Singleton<ClientSocket> , SocketInputProvide
    {
        public const int BYTE_LENGTH = 60;
        private ClientWarp _warp = null;
        private bool isConnect = false;

        public void Init()
        {
            _warp = new ClientWarp();
            _warp.InitClient("127.0.0.1", 7000);
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
            for (int i = 0; i < this._keyStates.Length; i++)
            {
                this._keyStates[i] = KeyState.Up;
            }
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

        private byte[] memBytes = new byte[BYTE_LENGTH];
        private unsafe void HandleData(DataItem item)
        {
            Debug.Log(string.Format("协议：{0} ",item.protocol));
            try
            {
                Marshal.Copy(item.GetBuffer(), memBytes, 0, item.bufferLength);
                // TODO 重用stream 防止GC  BYTE_LENGTH应该在协议中带 这里临时使用
                using (MemoryStream stream = new MemoryStream(memBytes, 0, item.bufferLength)) 
                {
                    KeyChange change = ProtoBuf.Serializer.Deserialize<KeyChange>(stream);
                    for (int i = 0; i < change.keyDatas.Count; i++)
                    {
                        Debug.Log(string.Format("key:{0}, keyState:{1}", change.keyDatas[i].key,
                            change.keyDatas[i].keyState));

                        this._keyStates[(int) change.keyDatas[i].key] = change.keyDatas[i].keyState;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        #region 按键数据

        private KeyState[] _keyStates = new KeyState[(int)KeyType.TypeCount];
        /// <summary>
        /// 按键状态
        /// </summary>
        public KeyState GetKeyState(KeyType type)
        {
            return _keyStates[(int) type];
        }
        

        #endregion
        
    }
}