using System.Collections.Generic;
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
            throw new System.NotImplementedException();
        }

        public bool GetKey(InputKeyType key)
        {
            if (this._keyState[(int) key] == KeyState.Down)
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

        private List<InputKeyType> _keyTypes = new List<InputKeyType>()
        {
            InputKeyType.Change, 
            InputKeyType.Fire
        };
        public void Update(float deltaTime)
        {
            for (int i = 0; i < this._keyTypes.Count; i++)
            {
                InputKeyType type = this._keyTypes[i];
                this._keyState[(int) type] = ProtocolHelper.GetKeyState(type);
            }
            
        }
    }
}