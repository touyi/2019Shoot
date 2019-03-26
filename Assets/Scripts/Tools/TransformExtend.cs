using UnityEngine;

namespace Tools
{
    public static class TransformExtend
    {
        public static Transform CustomFind(this Transform transform, string name)
        {
            if (transform == null)
            {
                return null;
            }

            return transform.Find(name);
        }

        public static T CustomGetComponent<T>(this Transform transform) where T : class
        {
            if (transform == null)
            {
                return null;
            }

            return transform.GetComponent<T>();
        }
    }
}