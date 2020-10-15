using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
     int SceneToLoad = 1;
    void OnTriggerEnter2D(Collider2D trigger)
    {
        GameManager.Instance.currentScene = SceneToLoad;
        SceneManager.LoadScene(SceneToLoad);
    }
}
