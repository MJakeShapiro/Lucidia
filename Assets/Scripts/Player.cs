﻿using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

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
    public InputMaster controls;
    // Holds player input
    private Vector2 moveDirection;
    // Holds player position
    public Rigidbody2D rb;
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
    [HideInInspector] public bool canDash = true;
    [HideInInspector] public RiftScript rift;
    [HideInInspector] public bool inRift = false;
    [HideInInspector] public bool boosted = false;

    //[Header("Attack")]
    //[SerializeField] private Transform attackPos;
    //[SerializeField] private LayerMask enemies;
    //[SerializeField] private float TOTAL_ATTACK_TIME;
    //private float attackTime;

    //public bool isAttacking = false;

    //Movement States
    [HideInInspector] public Vector2 airVelocity = Vector2.zero;
    [HideInInspector] public bool isJumping = false;
    private bool cancelJumpingQueue = false;
    private Direction direction = Direction.right;
    public Direction GetDirection { get => direction; }
    private State state = State.idle;

    [Header("Playtest bools")]
    [Tooltip("allows player to cancel jump early")] [SerializeField] private bool variableJump = false;
    [SerializeField] private bool startWithDash = false, diagonalDash = false;


    #endregion Properties

    #region Initialization
    private void Awake()
    {
        //GetSword bool to start without sword if on starting level
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            GetSword = false;
        }
        else
        {
            sword_sprite.SetActive(true);
        }

        rb = GetComponent<Rigidbody2D>();

        controls = new InputMaster();
        controls.Player.Movement.performed += _ => MovementInput();
        controls.Player.Jump.performed += _ => Jump();
        if (variableJump)
            controls.Player.Jump.canceled += _ => CancelJump();

        controls.Player.Dash.performed += _ => Dash();
        controls.Player.Attack.performed += _ => SwordBoop();

        RespawnPos();

    }

    private void OnEnable()
    {
        controls.Player.Movement.Enable();
        controls.Player.Jump.Enable();
        if (startWithDash)
            controls.Player.Dash.Enable();
        controls.Player.Attack.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Movement.Disable();
        controls.Player.Jump.Disable();
        controls.Player.Dash.Disable();
        controls.Player.Attack.Disable();
    }

    #endregion Initialization

    #region Update Methods
    private void Update()
    {
        DeathCheck();
        if (variableJump)
            JumpQueue();
        DashCounter();
        SwordBoopCounter();
        Recoil();
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
            animator.SetBool("IsRunning", true);
        }
        else if (moveDirection.x < 0.0f)
        {
            direction = Direction.left;
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
        Flip();
    }

    /// <summary>
    /// Moves player at a fixed rate based on moveDirection
    /// </summary>
    private void Movement()
    {
        if (!isDashing && !isRecoiling)
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
        animator.SetBool("IsJumping", true);
        if (GameManager.Instance.IsGrounded(feetPos))
        {
            //AudioManager.instance.PlaySound("jump-ploing");
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCounter = MIN_JUMP_COUNTER;
        }
    }

    /// <summary>
    /// Sets cancelJumpQueue to stop jump after minimum jump has been reached
    /// </summary>
    public void CancelJump()
    {
        animator.SetBool("IsJumping", false);
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
    /// Checks if players needs to be flipped and flips based on direction
    /// </summary>
    public void Flip()
    {
        Vector2 localScale = transform.localScale;
        if ((direction == Direction.left && localScale.x > 0) || (direction == Direction.right && localScale.x < 0))
        {
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    public void Launch()
    {
        if (!isDashing)
        {
            animator.SetBool("IsDashing", true);
            AudioManager.instance.PlaySound("dash2");
            isDashing = true;
            rb.gravityScale = 0.0f;
        }
            dashTime = TOTAL_DASH_TIME;
            AudioManager.instance.PlaySound("rift-dash");
            if (direction == Direction.right)
            {
                rb.velocity = Vector2.right * dashSpeed * 3;
            }
            if (direction == Direction.left)
            {
                rb.velocity = Vector2.left * dashSpeed * 3;
            }
            else if (direction == Direction.up)
            {
                rb.velocity = Vector2.up * dashSpeed * 3;
            }
            else if (direction == Direction.down && !GameManager.Instance.IsGrounded(feetPos))
            {
                rb.velocity = Vector2.down * dashSpeed * 3;
            }
            ResetDash();
    }
    #endregion Movement

    #region Abilities

    #region Dash
    /// <summary>
    /// Dashes player according to dashSpeed. DiagonalDash option.
    /// </summary>
    public void Dash()
    {
        if (canDash)
        {
            isDashing = true;
            animator.SetBool("IsDashing", true);
            //AudioManager.instance.PlaySound("dash2");

            if (!GameManager.Instance.IsGrounded(feetPos))
                hasAirDashed = true;

            dashTime = TOTAL_DASH_TIME;
            rb.gravityScale = 0.0f;

            GameObject DashEffectToDestroy = Instantiate(dashEffect, transform.position, Quaternion.identity);
            Destroy(DashEffectToDestroy, 0.2f);

            if (inRift)
                return;

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
                if (!boosted)
                {
                    canDash = false;
                    dashCooldown = MIN_DASH_COOLDOWN;
                }
                boosted = false;
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

    public void ResetDash()
    {
        canDash = true;
        dashCooldown = 0.0f;

    }
    public void CancelDash()
    {
        dashTime = 0.0f;
    }
    #endregion Dash

    #region Sword Attack

    [Header("Attack")]
    [SerializeField] private Transform horizontalAttackPos;
    [SerializeField] private Transform upAttackPos;
    [SerializeField] private Transform downAttackPos;
    [SerializeField] private LayerMask enemies;
    [SerializeField] private float horizontalAttackRangeX, horizontalAttackRangeY;
    [SerializeField] private float verticalAttackRangeX, verticalAttackRangeY;
    [SerializeField] private float TOTAL_ATTACK_TIME;
    [SerializeField] private float attackHorRecoil, RECOIL_DURATION;
    private float recoilTime;
    RecoilDir recoilDir;
    private bool isRecoiling;
    private float attackTime;

    public bool isAttacking = false;

    private void SwordBoop()
    {
 
        if (attackTime <= 0)
        {
            isAttacking = true;

            attackTime = TOTAL_ATTACK_TIME;
            Collider2D[] enemiesHit;
            if (direction == Direction.up)
            {
                enemiesHit = Physics2D.OverlapBoxAll(upAttackPos.position, new Vector2(verticalAttackRangeX, verticalAttackRangeY), 0.0f, enemies);
            }
            if (direction == Direction.down && GameManager.Instance.IsGrounded(feetPos))
                return;
            else if (direction == Direction.down)
            {
                enemiesHit = Physics2D.OverlapBoxAll(downAttackPos.position, new Vector2(verticalAttackRangeX, verticalAttackRangeY), 0.0f, enemies);
            }
            else
                enemiesHit = Physics2D.OverlapBoxAll(horizontalAttackPos.position, new Vector2(horizontalAttackRangeX, horizontalAttackRangeY), 0.0f, enemies);

            if (Physics2D.OverlapBox(horizontalAttackPos.position, new Vector2(horizontalAttackRangeX, horizontalAttackRangeY), 0.0f, enemies) || Physics2D.OverlapBox(horizontalAttackPos.position, new Vector2(horizontalAttackRangeX, horizontalAttackRangeY), 0.0f, GameManager.Instance.ground))
            {
                recoilTime = RECOIL_DURATION;
                isRecoiling = true;
                if (direction == Direction.left)
                    recoilDir = RecoilDir.right;
                if (direction == Direction.right)
                    recoilDir = RecoilDir.left;
            }

            for (int i = 0; i < enemiesHit.Length; i++)
            {
                Destroy(enemiesHit[i].gameObject);
                Debug.Log("SMHMACK");
            }

            //Apply knockback(TODO)
           
        }
    }

    private void SwordBoopCounter()
    {
        if (isAttacking)
        {
            animator.SetBool("IsAttacking", true);
            sword_sprite.SetActive(false);
            if (attackTime > 0)
            {
                attackTime -= Time.deltaTime;
            }
            else
            {
                isAttacking = false;
                animator.SetBool("IsAttacking", false);
                sword_sprite.SetActive(true);
            }
        }
    }

    public void Recoil()
    {
        if (isRecoiling)
        {
            if (recoilTime > 0)
            {
                if (recoilDir == RecoilDir.left)
                    rb.velocity = (double)rb.velocity.x <= -(double)attackHorRecoil ? new Vector2(rb.velocity.x - attackHorRecoil, rb.velocity.y) : new Vector2(-attackHorRecoil, rb.velocity.y);
                if (recoilDir == RecoilDir.right)
                    rb.velocity = (double)rb.velocity.x >= (double)attackHorRecoil ? new Vector2(rb.velocity.x + attackHorRecoil, rb.velocity.y) : new Vector2(attackHorRecoil, rb.velocity.y);

                recoilTime -= Time.deltaTime;
            }
            else
            {
                isRecoiling = false;
            }
        }
    }

    public void SetRecoil(RecoilDir dirToRecoil)
    {
        isRecoiling = true;
        recoilDir = dirToRecoil;
    }

    // ONLY FOR DEBUGGING AND TESTING
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(upAttackPos.position, new Vector3(verticalAttackRangeX, verticalAttackRangeY, 0.0f));

        Gizmos.DrawWireCube(downAttackPos.position, new Vector3(verticalAttackRangeX, verticalAttackRangeY, 0.0f));

        Gizmos.DrawWireCube(horizontalAttackPos.position, new Vector3(horizontalAttackRangeX, horizontalAttackRangeY, 0.0f));


    }

    #endregion Sword Attack

    #endregion Abilities

    #region Death

    [SerializeField] Vector2 launchPower;
    public void Die()
    {
        OnDisable();
        GameManager.Instance.playerRespawn = true;
        GameManager.Instance.ReloadScene();
    }

    private void DeathCheck()
    {
        if (rb.IsTouchingLayers(enemies))
        {
            Die();
        }
    }

    private void RespawnPos()
    {
        Debug.Log("In RespawnPos");
        if (GameManager.Instance.playerRespawn)                                                         // If player has died and is being respawned
            transform.position = GameManager.Instance.lastCheckpointPos;    // Set their position to the last checkpoint
        else
            GameManager.Instance.lastCheckpointPos = transform.position;    // Else a new scene is loaded, thus new checkpoint is the starting pos
    }

    #endregion Death

    #region FallDetector
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "FallDetector")
        {
            Debug.Log("Fall Detected!");
            Die();
        }
    }
    #endregion FallDetector
}