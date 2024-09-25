using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int _scene;

    private UIController _uiController;

    private void Start()
    {
        var controller = GameObject.FindGameObjectWithTag("GameController");
        _uiController = GameObject.Find("Canvas").GetComponent<UIController>();

        Destroy(controller);

        DontDestroyOnLoad(this.gameObject);
        PlayerController.PlayerDies += PlayerController_PlayerDies;
        PlayerController.PlayerExits += PlayerController_PlayerExits;
    }

    private void PlayerController_PlayerExits()
    {
        NextLevel();
    }

    private void PlayerController_PlayerDies()
    {

        _uiController.GetComponent<UIController>().ShowMenuPanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    private void Restart()
    {
        var scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene);
    }

    private void NextLevel()
    {
        var scene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(scene);
    }
}