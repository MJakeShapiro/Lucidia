using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    [SerializeField] private LayerMask ground;
    [SerializeField] public float checkRadius;



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
    /// Checks if player's feet are on ground
    /// </summary>
    /// <returns>True if player is on ground. False otherwise</returns>
    public bool IsGrounded(Transform feetPos)
    {
        return Physics2D.OverlapCircle(feetPos.position, checkRadius, ground);
    }
}
