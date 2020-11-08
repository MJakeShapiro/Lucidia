using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public LayerMask ground;
    public LayerMask wall;
    public float checkRadius;
    public SceneLoader sceneLoader;
    public Vector2 lastCheckpointPos;
    public bool changingScenes = false;
    public bool playerRespawn = false;


    //setup of singleton entity
    private static GameManager instance;

    //setup of singleton entity
    public static GameManager Instance
    {
        get { return instance ?? (instance = new GameObject("GameManager").AddComponent<GameManager>()); }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
        player = FindObjectOfType<Player>();
    }

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("levelLoaded");
        playerRespawn = false;
        player = FindObjectOfType<Player>();
    }

    /// <summary>
    /// Checks if object's feet are on ground
    /// </summary>
    /// <returns>True if object is on ground. False otherwise</returns>
    public bool IsGrounded(Transform feetPos)
    {
        return Physics2D.OverlapCircle(feetPos.position, checkRadius, ground);
    }
    

    /// <summary>
    /// Loads scene with fade animation
    /// </summary>
    /// <param name="sceneToLoad">
    /// The Scene to be loaded
    /// </param>
    public void ChangeScene(string sceneToLoad)
    {
        instance.sceneLoader.FadeToScene(sceneToLoad);
    }

    /// <summary>
    /// Reloads current active scene
    /// </summary>
    public void ReloadScene()
    {
        instance.sceneLoader.FadeToScene(SceneManager.GetActiveScene().name);
        //player.transform.position = lastCheckpointPos;
    }    
}
