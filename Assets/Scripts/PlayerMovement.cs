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

    [SerializeField] private float moveSpeed = 5.0f, jumpForce = 3.0f;
    [Tooltip("Minimum jump time in seconds")] [SerializeField] private float MIN_JUMP_COUNTER = 0.2f;
    private float jumpCounter = 0;

    [SerializeField] private float dashSpeed = 50.0f;
    [SerializeField] public float startDashTime;
    private float dashTime;

    [SerializeField] private float MIN_DASH_COOLDOWN;
    private float dashCooldown;
    private bool canDash;


    public GameObject dashEffect;


    // Holds player position
    private Rigidbody2D rb;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask ground;
    private int direction;

    //Movement States
    private bool isJumping = false;
    private bool cancelJumpingQueue = false;
    private bool isDashing = false;
   [SerializeField] private bool facingLeft = false, facingRight = true, facingUp = false, facingDown = false;

    [Space]
    [Tooltip("allows player to cancel jump early")][SerializeField] private bool variableJump = true;
    [SerializeField] private bool useVelocity = false;
    [SerializeField] private bool startWithDash = false;


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
            if (useVelocity)
                rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
            else
                rb.MovePosition(rb.position + (moveDirection * moveSpeed * Time.fixedDeltaTime));
        }
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCounter = MIN_JUMP_COUNTER;
        }
    }

    private void CancelJump()
    {
        isJumping = false;
        cancelJumpingQueue = true;
    }

    private void JumpQueue()
    {
        if (isJumping || cancelJumpingQueue)
        {
            jumpCounter -= Time.deltaTime;
        }
        if (cancelJumpingQueue && jumpCounter <= 0.0f)
        {
            cancelJumpingQueue = false;
            if (rb.velocity.y > 0.0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            }
        }
    }

    private void Dash()
    {
        if (canDash)
        {
            isDashing = true;
            if (facingRight)
            {
                rb.velocity = Vector2.right * dashSpeed;
            }
            else if (facingLeft)
            {
                rb.velocity = Vector2.left * dashSpeed;
            }
        }
    }

    private void DashCounter()
    {
        if (isDashing)
        {
            if (dashTime <= 0.0f)
            {
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
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

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(feetPos.position, checkRadius, ground);
    }

    #endregion Update Methods

}