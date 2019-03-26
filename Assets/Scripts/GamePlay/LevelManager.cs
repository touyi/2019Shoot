using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePlay
{
    public class LevelManager
    {
        // TODO
        public void InitScence()
        {
            SceneManager.LoadScene("Scences/new");
        }

        public Vector3 GetLocalPlayerPos(int index = 0)
        {
            // GC 优化
            GameObject go = GameObject.Find(string.Format("LocalPlayerPos{0}", index));
            if (go == null)
            {
                return Vector3.zero;
            }

            return go.transform.position;
        }

        public Transform GetLocalPlayerBase(int index = 0)
        {
            GameObject go = GameObject.Find(string.Format("LocalPlayerPos{0}", index));
            if (go == null)
            {
                return null;
            }

            return go.transform;
        }
    }
}