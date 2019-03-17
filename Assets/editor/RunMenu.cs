using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace editor
{
    public static class RunMenu
    {
        [MenuItem("Run/运行")]
        public static void Run()
        {
            EditorApplication.playmodeStateChanged += RunMenu.OnChange;
            EditorApplication.isPlaying = true;
            
            
        }

        public static void OnChange()
        {
            if (EditorApplication.isPlaying == true && EditorApplication.isPlayingOrWillChangePlaymode == true)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}