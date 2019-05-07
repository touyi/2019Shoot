using System.Collections.Generic;
using Message;
using MessageSystem;
using Protocol;
using Tools;
using UnityEngine;

namespace NetInput
{
    public class SInput : IInput
    {
        public enum KeyState
        {
            Down,
            KeepDown,
            Up,
            KeepUp,
        }
        private List<InputKeyType> _keyTypes = new List<InputKeyType>()
        {
            InputKeyType.Change, 
            InputKeyType.Fire
        };
        private KeyState[] _keyState;
        public SInput()
        {
            _keyState = new KeyState[(int)InputKeyType.Count];
            for (int i = 0; i < _keyState.Length; i++)
            {
                _keyState[i] = KeyState.KeepUp;
            }
        }
        public bool GetKeyDown(InputKeyType key)
        {
            if (this._keyState[(int) key] == KeyState.Down)
            {
                return true;
            }

            return false;
        }

        public bool GetKey(InputKeyType key)
        {
            Debug.Log(this._keyState[(int) key]);
            if (this._keyState[(int) key] == KeyState.KeepDown)
            {
                return true;
            }

            return false;
        }

        public float GetAxis(InputKeyType key)
        {
            throw new System.NotImplementedException();
        }

        public Vector2 GetAxis2D(InputKeyType key)
        {
            throw new System.NotImplementedException();
        }

        public Vector3 GetAxis3D(InputKeyType key)
        {
            return Vector3.forward;
        }

        public Vector4 GetAxis4D(InputKeyType key)
        {
            throw new System.NotImplementedException();
        }

        public void Init()
        {
            NetMessage.Instance.RegistNetListener(EProtocol.KeyChange , this.KeyChangeNetMessage);
        }

        private void KeyChangeNetMessage(EventParam param)
        {
            if (param.type != EProtocol.KeyChange)
            {
                return;
            }

            KeyChange change = param.message as KeyChange;
            if (change == null)
            {
                return;
            }

            for (int i = 0; i < change.keyDatas.Count; i++)
            {
                InputKeyType ukeyType = ProtocolHelper.TransitionKeyTypeU(change.keyDatas[i].key);
                KeyState uKeyState = ProtocolHelper.TransitionKeyState(change.keyDatas[i].keyState);
                this._keyState[(int) ukeyType] = uKeyState;
            }
        }

        
        public void Update(float deltaTime)
        {
//            for (int i = 0; i < this._keyTypes.Count; i++)
//            {
//                InputKeyType type = this._keyTypes[i];
//                this._keyState[(int) type] = ProtocolHelper.GetKeyState(type);
//            }
            for (int i = 0; i < this._keyState.Length; i++)
            {
                if (this._keyState[i] == KeyState.Down)
                {
                    this._keyState[i] = KeyState.KeepUp;
                }
                else if (this._keyState[i] == KeyState.Up)
                {
                    this._keyState[i] = KeyState.KeepUp;
                }
            }
        }
    }
}