using UnityEngine;

namespace NetInput
{
    public interface IInput
    {
        bool GetKeyDown(InputKeyCode key);
        bool GetKey(InputKeyCode key);
        float GetAxis(InputKeyCode key);
        Vector2 GetAxis2D(InputKeyCode key);
        Vector3 GetAxis3D(InputKeyCode key);
        Vector4 GetAxis4D(InputKeyCode key);
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
                            _input = new TempInput();
                        }
                    }
                }

                return _input;
            }
        }
    }
}