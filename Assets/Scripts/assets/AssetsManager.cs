using UnityEngine;
using Wrapper;

namespace assets
{
    public class AssetsManager : Singleton<AssetsManager>
    {
        /// <summary>
        /// 加载prefab
        /// </summary>
        /// <param name="path">在Resources/Prefab下的路径 不加Resource/Prefab</param>
        /// <returns></returns>
        public GameObject LoadPrefab(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("LoadPrefab error path is null or empty");
                return null;
            }

            path = "Prefabs/" + path.Replace("//", "/").Replace("\\", "/");
            GameObject go = this.LoadPrefab(path) as GameObject;
            if (go == null)
            {
                Debug.LogError(string.Format("LoadPrefab error path:{0}", path));
            }

            return go;
        }

        private UnityEngine.Object Load(string path)
        {
            return Resources.Load<UnityEngine.Object>(path);
        }
    }
}