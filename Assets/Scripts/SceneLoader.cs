using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Animator animator;
    public GameObject loadingScreen;
    public Slider slider;

    private string sceneToLoad;
    int StartScene = 0;


    public void FadeToScene(string sceneName)
    {
        if (GameManager.Instance.changingScenes)
            return;
        Time.timeScale = 0.0f;
        GameManager.Instance.changingScenes = true;
        sceneToLoad = sceneName;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(sceneToLoad);
        Time.timeScale = 1.0f;
       
        animator.SetTrigger("FadeIn");
        GameManager.Instance.changingScenes = false;
    }

    /// <summary>
    /// Loads a scene with a loading bar
    /// </summary>
    /// <param name="sceneName">
    /// The Scene to be loaded
    /// </param>
    public void LoadingScene(string sceneName)
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;

            yield return null;
        }
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
