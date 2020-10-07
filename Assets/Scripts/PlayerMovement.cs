using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Properties

    // Takes players input
    private InputMaster controls;

    // Holds player input
    private Vector2 moveDirection;

    public GameObject dashEffect;

    // Holds player position
    private Rigidbody2D rb;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask ground;

    [Space]
    [SerializeField] private float moveSpeed = 5.0f, jumpForce = 3.0f;
    [Tooltip("Minimum jump time in seconds")] [SerializeField] private float MIN_JUMP_COUNTER = 0.2f;
    private float jumpCounter = 0;

    [Space]
    [SerializeField] private float dashSpeed = 50.0f;
    [SerializeField] public float TOTAL_DASH_TIME;
    private float dashTime;

    [SerializeField] private float MIN_DASH_COOLDOWN;
    private float dashCooldown;
    private bool canDash;

    //Movement States
    private bool isJumping = false;
    private bool cancelJumpingQueue = false;
    private bool isDashing = false;
    private bool facingLeft = false, facingRight = true, facingUp = false, facingDown = false;

    [Space]
    [Tooltip("allows player to cancel jump early")][SerializeField] private bool variableJump = true;
    [SerializeField] private bool startWithDash = false, diagonalDash = false;


    #endregion Properties

    #region Initialization
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        controls = new InputMaster();
        controls.Player.Movement.performed += _ => MovementInput();
        controls.Player.Jump.performed += _ => Jump();
        if (variableJump)
            controls.Player.Jump.canceled += _ => CancelJump();
        controls.Player.Dash.performed += _ => Dash();


    }

    void OnEnable()
    {
        controls.Player.Movement.Enable();
        controls.Player.Jump.Enable();
        if (startWithDash)
            controls.Player.Dash.Enable();
    }

    void OnDisable()
    {
        controls.Player.Movement.Disable();
        controls.Player.Jump.Disable();
        controls.Player.Dash.Disable();
    }

    #endregion Initialization

    #region Update Methods
    private void Update()
    {
        if(variableJump)
            JumpQueue();
        DashCounter();

    }

    private void FixedUpdate()
    {
        Movement();
    }

    /// <summary>
    /// Takes player input every frame and saves for fixed movement
    /// </summary>
    private void MovementInput()
    {
        moveDirection = controls.Player.Movement.ReadValue<Vector2>();
        if (moveDirection.x > 0.0f)
        {
            facingRight = true;
            facingLeft = false;
            facingUp = false;
            facingDown = false;
        }
        else if (moveDirection.x < 0.0f)
        {
            facingRight = false;
            facingLeft = true;
            facingUp = false;
            facingDown = false;
        }
        else if (moveDirection.y > 0.0f)
        {
            facingRight = false;
            facingLeft = false;
            facingUp = true;
            facingDown = false;
        }
        else if (moveDirection.y < 0.0f)
        {
            facingRight = false;
            facingLeft = false;
            facingUp = false;
            facingDown = true;
        }
    }

    /// <summary>
    /// Moves player at a fixed rate based on moveDirection
    /// </summary>
    private void Movement()
    {
        if (!isDashing)
        {
                rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
        }
    }

    /// <summary>
    /// Jumps Player and sets counter for Variable Jump
    /// </summary>
    private void Jump()
    {
        if (IsGrounded())
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCounter = MIN_JUMP_COUNTER;
        }
    }

    /// <summary>
    /// Sets cancelJumpQueue to stop jump after minimum jump has been reached
    /// </summary>
    private void CancelJump()
    {
        isJumping = false;
        cancelJumpingQueue = true;
    }

    /// <summary>
    /// Decreases jumpCounter to ensure MIN_JUMP_COUNTER
    /// </summary>
    private void JumpQueue()
    {
        if (isJumping || cancelJumpingQueue)                        // Is the player still jumping or have they released the jump button?
        {
            jumpCounter -= Time.deltaTime;
        }
        if (cancelJumpingQueue && jumpCounter <= 0.0f)              // Has player reached minimum jump height?
        {
            cancelJumpingQueue = false;
            if (rb.velocity.y > 0.0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            }
        }
    }

    /// <summary>
    /// Dashes player according to dashSpeed. DiagonalDash option.
    /// </summary>
    private void Dash()
    {
        if (canDash)
        {
            isDashing = true;
            rb.gravityScale = 0.0f;
            if (diagonalDash)
                rb.velocity = new Vector2(moveDirection.x * dashSpeed, moveDirection.y * dashSpeed);
            else
            {
                if (facingRight)
                {
                    rb.velocity = Vector2.right * dashSpeed;
                }
                else if (facingLeft)
                {
                    rb.velocity = Vector2.left * dashSpeed;
                }
                else if (facingUp)
                {
                    rb.velocity = Vector2.up * dashSpeed;
                }
                else if (facingDown && !IsGrounded())
                {
                    rb.velocity = Vector2.down * dashSpeed;
                }
            }
        }
    }

    /// <summary>
    /// Manages dash distance according to TOTAL_DASH_TIME
    /// </summary>
    private void DashCounter()
    {
        if (isDashing)
        {
            if (dashTime <= 0.0f)
            {
                dashTime = TOTAL_DASH_TIME;
                rb.velocity = Vector2.zero;
                rb.gravityScale = 4.0f;
                isDashing = false;
                canDash = false;
                dashCooldown = MIN_DASH_COOLDOWN;
            }
            else
            {
                dashTime -= Time.deltaTime;
            }
        }

        if (!canDash)
            dashCooldown -= Time.deltaTime;
        if (IsGrounded())
        {
            if (dashCooldown <= 0.0f)
                canDash = true;
        }
    }


    /// <summary>
    /// Checks if player's feet are on ground
    /// </summary>
    /// <returns>True if player is on ground. False otherwise</returns>
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(feetPos.position, checkRadius, ground);
    }

    #endregion Update Methods

}