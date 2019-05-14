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
        private Vector3 _vector3 = Vector3.forward;
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
            if (key == InputKeyType.DirVector)
            {
                return this._vector3;
            }
            return Vector2.zero;
        }

        public Vector3 GetAxis3D(InputKeyType key)
        {
            if (key == InputKeyType.DirVector)
            {
                return this._vector3;
            }
            return Vector3.zero;
        }

        public Vector4 GetAxis4D(InputKeyType key)
        {
            throw new System.NotImplementedException();
        }

        public void Init()
        {
            NetMessage.Instance.RegistNetListener(EProtocol.KeyChange , this.KeyChangeNetMessage);
            NetMessage.Instance.RegistNetListener(EProtocol.MobileDir , this.RoteNetMessage);
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

        private void RoteNetMessage(EventParam param)
        {
            if (param.type != EProtocol.MobileDir)
            {
                return;
            }
            VecList list = param.message as VecList;
            if (list == null)
            {
                return;
            }

            for (int i = 0; i < list.vec.Count; i++)
            {
                Vec3 vec = list.vec[i];
                Vector2 uVec3;
                uVec3.x = vec.x;
                uVec3.y = vec.y;
                Debug.Log(uVec3);
                InputDirChange.Instance.AddNewPos(uVec3);
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

            this._vector3 = InputDirChange.Instance.GetLookAtScreenPos();
        }
    }
}