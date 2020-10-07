using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public PlayerMovement playerMovement;


    //setup of singleton entity
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance ?? (instance = new GameObject("GameManager").AddComponent<GameManager>()); }
    }
    private void Awake()
    {
        instance = this;
        playerMovement = player.GetComponent<PlayerMovement>();
    }
}
