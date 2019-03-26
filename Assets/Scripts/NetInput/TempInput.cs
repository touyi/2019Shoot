﻿using UnityEngine;

namespace NetInput
{
    public class TempInput : IInput
    {
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

        private Vector3 dir = Vector3.zero;
        public Vector3 GetAxis3D(InputKeyCode key)
        {
            switch (key)
            {
                    case InputKeyCode.DirVector:
                        return dir;
                        break;
            }

            Debug.LogError(string.Format("InputKeyError keycode {0}", key.ToString()));
            return Vector3.zero;

        }

        public Vector4 GetAxis4D(InputKeyCode key)
        {
            throw new System.NotImplementedException();
        }

        public void Update(float deltaTime)
        {
            float x = Input.GetAxis("Mouse X") * deltaTime;
            float y = Input.GetAxis("Mouse Y") * deltaTime;
            dir.x += x;
            dir.y += y;
        }
    }
}