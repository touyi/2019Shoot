using UnityEngine;

namespace NetInput
{
    public class SInput : IInput
    {
        enum KeyState
        {
            Down,
            KeepDown,
            Up,
            KeepUp,
        }
        private ClientWarp _clientWarp = null;
        private KeyState[] isKeyDown;
        public SInput()
        {
            _clientWarp = new ClientWarp();
            isKeyDown = new KeyState[(int)InputKeyCode.Count];
            for (int i = 0; i < isKeyDown.Length; i++)
            {
                isKeyDown[i] = KeyState.KeepUp;
            }
        }
        public bool GetKeyDown(InputKeyCode key)
        {
            throw new System.NotImplementedException();
        }

        public bool GetKey(InputKeyCode key)
        {
            throw new System.NotImplementedException();
        }

        public float GetAxis(InputKeyCode key)
        {
            throw new System.NotImplementedException();
        }

        public Vector2 GetAxis2D(InputKeyCode key)
        {
            throw new System.NotImplementedException();
        }

        public Vector3 GetAxis3D(InputKeyCode key)
        {
            throw new System.NotImplementedException();
        }

        public Vector4 GetAxis4D(InputKeyCode key)
        {
            throw new System.NotImplementedException();
        }

        public void Update(float deltaTime)
        {
            // Next
        }
    }
}