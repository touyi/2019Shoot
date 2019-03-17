using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace editor
{
    public static class RunMenu
    {
        [MenuItem("Run/运行")]
        public static void Run()
        {
            EditorSceneManager.OpenScene("Assets/Scences/Start.unity");
            EditorApplication.isPlaying = true;
        }
    }
}