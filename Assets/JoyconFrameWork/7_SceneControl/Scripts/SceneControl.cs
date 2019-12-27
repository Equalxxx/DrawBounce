using UnityEngine.SceneManagement;

namespace JoyconFramework
{
    public static class SceneControl {
    
        public static string nextScene;

        public static void LoadNextScene(string sceneName)
        {
            nextScene = sceneName;
            SceneManager.LoadScene("LoadingScene");
        }
    }
}
