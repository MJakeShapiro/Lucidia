using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Art/Animation


    //Animator/Art variables for the player
    public Animator animator;
    private bool m_FacingRight = true;
    public bool GetSword;
    public GameObject sword_sprite;


    #endregion Art/Animation

    #region Properties



    // Takes players input
    private InputMaster controls;
    // Holds player input
    private Vector2 moveDirection;
    // Holds player position
    private Rigidbody2D rb;
    [SerializeField] public Transform feetPos;

    [SerializeField] private float moveSpeed = 6.0f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 20.0f;
    [Tooltip("Minimum jump time in seconds")] [SerializeField] private float MIN_JUMP_COUNTER = 0.17f;
    private float jumpCounter = 0;

    [Header("Dash")]
    [SerializeField] private GameObject dashEffect;
    [SerializeField] private float dashSpeed = 50.0f;
    [SerializeField] public float TOTAL_DASH_TIME = 0.1f;
    [SerializeField] private float MIN_DASH_COOLDOWN = 0.5f;
    [HideInInspector] public bool isDashing = false, hasAirDashed = false;
    private float dashTime;
    private float dashCooldown = 0.0f;
    private bool canDash = true;

    //Movement States
    private Vector2 airVelocity = Vector2.zero;
    [HideInInspector] public bool isJumping = false;
    private bool cancelJumpingQueue = false;
    private Direction direction = Direction.right;

    [Header("Playtest bools")]
    [Tooltip("allows player to cancel jump early")] [SerializeField] private bool variableJump = false;
    [SerializeField] private bool startWithDash = false, diagonalDash = false;


    #endregion Properties

    #region Initialization
    private void Awake()
    {
        //GetSword bool to start without sword
        GetSword = false;
        sword_sprite.SetActive(false);

        rb = GetComponent<Rigidbody2D>();

        controls = new InputMaster();
        controls.Player.Movement.performed += _ => MovementInput();
        controls.Player.Jump.performed += _ => Jump();
        if (variableJump)
            controls.Player.Jump.canceled += _ => CancelJump();

        controls.Player.Dash.performed += _ => Dash();


    }

    private void OnEnable()
    {
        controls.Player.Movement.Enable();
        controls.Player.Jump.Enable();
        if (startWithDash)
            controls.Player.Dash.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Movement.Disable();
        controls.Player.Jump.Disable();
        controls.Player.Dash.Disable();
    }

    #endregion Initialization

    #region Update Methods
    private void Update()
    {
        if (variableJump)
            JumpQueue();
        DashCounter();

        if (GetSword == true)
        {
            sword_sprite.SetActive(true);
        }

    }

    private void FixedUpdate()
    {
        Movement();
    }

    #endregion Update Methods

    #region Movement
    /// <summary>
    /// Takes player input every frame and saves for fixed movement
    /// </summary>
    private void MovementInput()
    {
        moveDirection = controls.Player.Movement.ReadValue<Vector2>();
        if (moveDirection.x > 0.0f)
        {
            direction = Direction.right;
            if (m_FacingRight == false)
            {
                Flip();
            }
            animator.SetBool("IsRunning", true);
        }
        else if (moveDirection.x < 0.0f)
        {
            direction = Direction.left;
            if(m_FacingRight == true)
            {
                Flip();
            }
            animator.SetBool("IsRunning", true);
        }
        else if (moveDirection.y > 0.0f)
        {
            direction = Direction.up;
        }
        else if (moveDirection.y < 0.0f)
        {
            direction = Direction.down;
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }

    /// <summary>
    /// Moves player at a fixed rate based on moveDirection
    /// </summary>
    private void Movement()
    {
        
        if (!isDashing)
        {
            animator.SetBool("IsDashing", false);
            if (!GameManager.Instance.IsGrounded(feetPos))
            {
                if (moveDirection.x != 0)
                {
                    rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
                    airVelocity = rb.velocity;
                }
                else
                {
                    rb.velocity = new Vector2(airVelocity.x, rb.velocity.y);
                }
            }
            else
            {
                airVelocity = Vector2.zero;
                rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
            }
            
        }
           
        }

    /// <summary>
    /// Jumps Player and sets counter for Variable Jump
    /// </summary>
    private void Jump()
    {
        if (GameManager.Instance.IsGrounded(feetPos))
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
    #endregion Movement

    #region Abilities

    #region Dash
    /// <summary>
    /// Dashes player according to dashSpeed. DiagonalDash option.
    /// </summary>
    private void Dash()
    {
        if (canDash)
        {
            animator.SetBool("IsDashing", true);
            if (!GameManager.Instance.IsGrounded(feetPos))
                hasAirDashed = true;

            isDashing = true;
            dashTime = TOTAL_DASH_TIME;
            rb.gravityScale = 0.0f;

            GameObject DashEffectToDestroy = Instantiate(dashEffect,transform.position,Quaternion.identity);
            Destroy(DashEffectToDestroy, 0.15f);

            if (diagonalDash)
                rb.velocity = new Vector2(moveDirection.x * dashSpeed, moveDirection.y * dashSpeed);
            else
            {
                if (direction == Direction.right)
                {
                    rb.velocity = Vector2.right * dashSpeed;
                    
                }
                else if (direction == Direction.left)
                {
                    rb.velocity = Vector2.left * dashSpeed;
                }
                else if (direction == Direction.up)
                {
                    rb.velocity = Vector2.up * dashSpeed;
                }
                else if (direction == Direction.down && !GameManager.Instance.IsGrounded(feetPos))
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
        if (dashCooldown <= 0.0f)
        {
            if (GameManager.Instance.IsGrounded(feetPos) || !hasAirDashed)
            {
                canDash = true;
                hasAirDashed = false;
            }

        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    #endregion Dash

    #endregion Abilities
}