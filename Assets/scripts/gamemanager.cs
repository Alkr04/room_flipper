using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class gamemanager : MonoBehaviour
{
    int scean;
    public int test = 0;
    GameObject canvas;
    //GameObject player;
    //public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] manager = GameObject.FindGameObjectsWithTag("GameController");
        canvas = GameObject.Find("Canvas");
        
        if (manager.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        test++;
        PlayerController.PlayerDies += PlayerController_PlayerDies;
    }

    private void PlayerController_PlayerDies()
    {
        //Debug.Log("test");
        canvas.GetComponent<UIController>().ShowMenuPanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            restart();
        }
    }

    public void quit()
    {
        Application.Quit();
    }
    public void restart()
    {
        //Destroy(player);
        scean = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scean);
    }
    public void nextlv()
    {
        scean = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(scean);
    }
}
