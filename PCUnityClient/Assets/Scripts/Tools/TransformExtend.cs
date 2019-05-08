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
        
        public static T CustomGetComponent<T>(this Transform transform, string path) where T : class
        {
            if (transform == null)
            {
                return null;
            }

            return transform.Find(path).CustomGetComponent<T>();
        }

        public static void CustomSetActive(this Transform transform, bool isActive)
        {
            if (transform != null && transform.gameObject.activeInHierarchy != isActive)
            {
                transform.gameObject.SetActive(isActive);
            }
        }
    }
}