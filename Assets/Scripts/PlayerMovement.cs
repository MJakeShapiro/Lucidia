using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public bool useVelocity = false;
    public float moveSpeed = 5.0f;

    // Used to limit player conntrol based on current path tile
    public bool canMoveUp = false, 
        canMoveDown = false, 
        canMoveLeft = false, 
        canMoveRight = false;

    // Takes players input
    InputMaster controls;
    //public InputAction moveAction = InputMaster.PlayerActions.Movement;

    // Holds player input
    private Vector2 moveDirection;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new InputMaster();
        controls.Player.Movement.started += OnMove;
        controls.Player.Movement.performed += OnMove;
        controls.Player.Movement.canceled += OnMove;
    }

    void OnEnable()
    {
        controls.Player.Movement.Enable();
    }

    void OnDisable()
    {
        controls.Player.Movement.Disable();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    /// <summary>
    /// Takes player input every scene and saves for fixed movement
    /// </summary>
    private void HandleInput()
    {
        moveDirection = controls.Player.Movement.ReadValue<Vector2>();
    }

    /// <summary>
    /// Moves player at a fixed rate based on moveDirection
    /// </summary>
    private void HandleMovement()
    {
        if (useVelocity)
        {
            rb.velocity = moveDirection * moveSpeed;
        }
        else
            rb.MovePosition(rb.position + (moveDirection * moveSpeed * Time.fixedDeltaTime));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                
                return;
            case InputActionPhase.Performed:
                HandleInput();
                return;
            case InputActionPhase.Canceled:

                return;
        }
    }
}