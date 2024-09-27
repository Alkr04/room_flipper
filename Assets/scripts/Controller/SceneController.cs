using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controller
{
    public static class SceneController
    {
        static SceneController()
        {
            Application.targetFrameRate = 60;
        }

        public static void Restart()
        {
            var scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene);
        }

        public static void StartGame()
        {
            SceneManager.LoadScene(1);
        }

        public static void NextLevel()
        {
            var scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene + 1);
        }

        public static void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public static void GoToMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}