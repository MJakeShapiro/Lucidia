using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    int SceneToLoad = 1;
    int StartScene = 0;
    void OnTriggerEnter2D(Collider2D trigger)
    {
        GameManager.Instance.currentScene = SceneToLoad;
        SceneManager.LoadScene(SceneToLoad);
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene(StartScene);
    }

    public void QuitGame()
    {
        Debug.Log("QUIITTTT!!!!");
        Application.Quit();
    }
}
