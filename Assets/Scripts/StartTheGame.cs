using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartTheGame : MonoBehaviour
{


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PressToStart();
    }

    void PressToStart()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(currentScene + 1);
        }
    }
}
