using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Properties
    [Header("Script Components")]
    [SerializeField] protected PlayerMovement playerMovement;
    [SerializeField] protected PlayerAbilities playerAbilities;
    [SerializeField] protected InputMaster controls;
    
    [Space]
    [SerializeField] private Transform feetPos;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask ground;
    protected Rigidbody2D rb;

    [Header("Movement")]
    [Space]
    [SerializeField] protected float moveSpeed = 5.0f, jumpForce = 3.0f;
    [Tooltip("Minimum jump time in seconds")] [SerializeField] protected float MIN_JUMP_COUNTER = 0.2f;
    protected float jumpCounter = 0;
    protected Vector2 moveDirection;
    protected Direction direction;
    protected bool isJumping = false;
    protected bool cancelJumpingQueue = false;

    [Space]
    [Tooltip("allows player to cancel jump early")] [SerializeField] protected bool variableJump = true;


    [Header("Dash")]
    public GameObject dashEffect;

    [Space]
    [SerializeField] protected float dashSpeed = 50.0f;
    [SerializeField] public float TOTAL_DASH_TIME;
    protected float dashTime;

    [SerializeField] protected float MIN_DASH_COOLDOWN;
    protected float dashCooldown;
    protected bool canDash;
    protected bool isDashing = false;

    [SerializeField] protected bool startWithDash = false, diagonalDash = false;

    #endregion Properties

    #region Initialization
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAbilities = GetComponent<PlayerAbilities>();
        playerMovement = GetComponent<PlayerMovement>();

        controls = new InputMaster();
        
    }

    #endregion Initialization

    #region General Functions
    /// <summary>
    /// Checks if player's feet are on ground
    /// </summary>
    /// <returns>True if player is on ground. False otherwise</returns>
    protected bool IsGrounded()
    {
        return Physics2D.OverlapCircle(feetPos.position, checkRadius, ground);
    }

    #endregion General Functions
}


