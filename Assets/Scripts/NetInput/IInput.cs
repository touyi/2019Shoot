using UnityEngine;

namespace NetInput
{
    public interface IInput
    {
        bool GetKeyDown(string keyName);
        bool GetKey(string keyName);
        float GetAxis(string name);
        Vector2 GetAxis2D(string name);
        Vector3 GetAxis3D(string name);
        Vector4 GetAxis4D(string name);
    }
}