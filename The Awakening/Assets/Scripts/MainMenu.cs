using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Game loading variables
    public string newGameScene;
    public GameObject continueButton;
    public string loadGameScene;

    // Use this for initialization
    void Start()
    {
        if (PlayerPrefs.HasKey("Current_Scene"))
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Start off from prior scene
    public void Continue()
    {
        SceneManager.LoadScene(loadGameScene);
    }

    // Start new game
    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    // Close game
    public void Exit()
    {
        Application.Quit();
    }
}
