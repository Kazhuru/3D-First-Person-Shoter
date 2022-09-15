using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void RestarCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadGameZone()
    {
        SceneManager.LoadScene("GameZone");
    }

    public void QuitGame()
    {
        Debug.Log("Unity Log: Application Quit");
        Application.Quit();
    }
}
