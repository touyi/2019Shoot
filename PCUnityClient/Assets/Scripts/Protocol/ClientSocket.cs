using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Message;
using MessageSystem;
using NetInput;
using Wrapper;
using Command = Message.Command;
using Debug = UnityEngine.Debug;

namespace Protocol
{

    public class ClientSocket : Singleton<ClientSocket>
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


        private void HandleData(DataItem item)
        {
            Debug.Log(string.Format("协议：{0} ", item.protocol));
            Marshal.Copy(item.GetBuffer(), memBytes, 0, item.bufferLength);
            using (MemoryStream stream = new MemoryStream(memBytes, 0, item.bufferLength))
            {
                //KeyChange change = ProtoBuf.Serializer.Deserialize<KeyChange>(stream);
                object msg = this.ParseBuffer(item.protocol, stream);
                NetMessage.Instance.LaunchNetMessage((EProtocol) item.protocol, msg);
            }

        }

        private object ParseBuffer(int protocol, MemoryStream stream)
        {
            EProtocol ep = (EProtocol) protocol;
            switch (ep)
            {
                case EProtocol.KeyChange:
                    return ProtoBuf.Serializer.Deserialize<KeyChange>(stream);
                    break;
                case EProtocol.NetCmd:
                    return ProtoBuf.Serializer.Deserialize<CommandList>(stream);

                    break;
                case EProtocol.MobileDir:
                    return ProtoBuf.Serializer.Deserialize<VecList>(stream);
                case EProtocol.IPExchange:
                    return ProtoBuf.Serializer.Deserialize<IPInfo>(stream);
                default: return null;
            }
        }
        public void SendGameEnd()
        {
            CommandList list = new CommandList();
            Message.Command cmd = new Message.Command();
            cmd.ctype = CmdType.GameEnd;
            list.commandDatas.Add(cmd);
            MemoryStream stream = new MemoryStream();

            ProtoBuf.Serializer.Serialize(stream, list);
            int size = (int)stream.Length;
            IntPtr mem = Marshal.AllocHGlobal(size);
            Marshal.Copy(stream.ToArray(), 0, mem, size);
            this._warp.SendData((int) EProtocol.NetCmd, mem, size);
        }
        public void RequestIP()
        {
            this._warp.SendData((int) EProtocol.IPExchange, new IntPtr(0), 0);
        }

        #region 按键数据

//        private KeyState[] _keyStates = new KeyState[(int)KeyType.TypeCount];
//        /// <summary>
//        /// 按键状态
//        /// </summary>
//        public KeyState GetKeyState(KeyType type)
//        {
//            return _keyStates[(int) type];
//        }
//
//        private void InitKeyState()
//        {
//            for (int i = 0; i < this._keyStates.Length; i++)
//            {
//                this._keyStates[i] = KeyState.Up;
//            }
//        }

        #endregion

    }
}