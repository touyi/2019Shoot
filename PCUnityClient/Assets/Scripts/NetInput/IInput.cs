using UnityEngine;

namespace NetInput
{
    public interface IInput
    {
        bool GetKeyDown(InputKeyType key);
        bool GetKey(InputKeyType key);
        float GetAxis(InputKeyType key);
        Vector2 GetAxis2D(InputKeyType key);
        Vector3 GetAxis3D(InputKeyType key);
        Vector4 GetAxis4D(InputKeyType key);
        void Init();
        void Update(float deltaTime);
    }

    public static class CurrentInput
    {
        private static IInput _input = null;
        
        private static object _lock = new object();

        public static IInput CurInput
        {
            get
            {
                if (_input == null)
                {
                    lock (_lock)
                    {
                        if (_input == null)
                        {
                            _input = new SInput();
                        }
                    }
                }

                return _input;
            }
        }
    }
}