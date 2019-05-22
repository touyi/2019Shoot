using System;
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
                case InputKeyType.Yes:
                    if (Input.GetKey(KeyCode.Space))
                    {
                        return true;
                    }

                    break;
                case InputKeyType.No:
                    if (Input.GetKey(KeyCode.Escape))
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

        private Vector3 pos = new Vector3(Screen.width / 2f, Screen.height / 2f, 2000);
        public Vector3 GetAxis3D(InputKeyType key)
        {
            switch (key)
            {
                    case InputKeyType.DirVector:
                        return pos;
                        break;
            }

            Debug.LogError(string.Format("InputKeyError keycode {0}", key.ToString()));
            return Vector3.zero;

        }

        public Vector4 GetAxis4D(InputKeyType key)
        {
            throw new System.NotImplementedException();
        }

        public void Init()
        {
        }

        public void Update(float deltaTime)
        {
            float x = Input.GetAxis("Mouse X") * deltaTime * 300;
            float y = Input.GetAxis("Mouse Y") * deltaTime * 300;
            pos.y += y;
            pos.x += x;
            pos.z = 2000;
            pos.x = Mathf.Clamp(pos.x, 0, Screen.width);
            pos.y = Mathf.Clamp(pos.y, 0, Screen.height);
        }
    }
}