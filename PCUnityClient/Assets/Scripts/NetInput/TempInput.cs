using UnityEngine;

namespace NetInput
{
    public class TempInput : IInput
    {
        public bool GetKeyDown(InputKeyCode key)
        {
            switch (key)
            {
                    case InputKeyCode.Fire:
                        if (Input.GetMouseButtonDown(0))
                        {
                            return true;
                        }
                        break;
            }

            return false;
        }

        public bool GetKey(InputKeyCode key)
        {
            switch (key)
            {
                case InputKeyCode.Fire:
                    if (Input.GetMouseButton(0))
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        public float GetAxis(InputKeyCode key)
        {
            throw new System.NotImplementedException();
        }

        public Vector2 GetAxis2D(InputKeyCode key)
        {
            throw new System.NotImplementedException();
        }

        private Vector3 dir = Vector3.forward;
        public Vector3 GetAxis3D(InputKeyCode key)
        {
            switch (key)
            {
                    case InputKeyCode.DirVector:
                        return dir.normalized;
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
            dir.y += y;
            dir.x += x;
        }
    }
}