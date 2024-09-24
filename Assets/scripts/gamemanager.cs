using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class gamemanager : MonoBehaviour
{
    int scean;
    public int test = 0;
    //public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] manager = GameObject.FindGameObjectsWithTag("GameController");
        
        if (manager.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        test++;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            restart();
        }
    }

    void quit()
    {
        Application.Quit();
    }
    void restart()
    {
        //Destroy(player);
        scean = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scean);
    }
}
