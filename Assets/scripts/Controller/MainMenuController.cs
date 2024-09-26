using UnityEngine;
using UnityEngine.UI;

namespace Controller
{
    public class MainMenuController : MonoBehaviour
    {
        private void Start()
        {
            GameObject.Find("startBtn").GetComponent<Button>().onClick.AddListener(SceneController.StartGame);
            GameObject.Find("exitBtn").GetComponent<Button>().onClick.AddListener(SceneController.Quit);
        }
    }
}