using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int SceneToLoad = 1;
    void OnTriggerEnter2D(Collider2D trigger)
    {
        SceneManager.LoadScene(SceneToLoad);
        GameManager.Instance.currentScene = SceneToLoad;
    }
}
