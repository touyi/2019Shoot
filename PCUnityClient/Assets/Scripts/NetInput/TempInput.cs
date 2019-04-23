using UnityEngine;

namespace NetInput
{
    public class TempInput : IInput
    {
        public bool GetKeyDown(InputKeyType key)
        {
            switch (key)
            {
                    case InputKeyType.Fire:
                        if (Input.GetMouseButtonDown(0))
                        {
                            return true;
                        }
                        break;
            }

            return false;
        }

        public bool GetKey(InputKeyType key)
        {
            switch (key)
            {
                case InputKeyType.Fire:
                    if (Input.GetMouseButton(0))
                    {
                        return true;
                    }
                    break;
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

        private Vector3 dir = Vector3.forward;
        public Vector3 GetAxis3D(InputKeyType key)
        {
            switch (key)
            {
                    case InputKeyType.DirVector:
                        return dir.normalized;
                        break;
            }

            Debug.LogError(string.Format("InputKeyError keycode {0}", key.ToString()));
            return Vector3.zero;

        }

        public Vector4 GetAxis4D(InputKeyType key)
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