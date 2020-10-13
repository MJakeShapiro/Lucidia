using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public LayerMask ground;
    public float checkRadius;



    //setup of singleton entity
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance ?? (instance = new GameObject("GameManager").AddComponent<GameManager>()); }
    }
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Checks if object's feet are on ground
    /// </summary>
    /// <returns>True if object is on ground. False otherwise</returns>
    public bool IsGrounded(Transform feetPos)
    {
        return Physics2D.OverlapCircle(feetPos.position, checkRadius, ground);
    }
}
