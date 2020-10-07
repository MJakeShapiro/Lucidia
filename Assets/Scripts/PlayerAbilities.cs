using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : Player
{
    #region Initialization
    void Awake()
    {
        controls.Player.Dash.performed += _ => Dash();
    }
    #endregion Initialization

    #region Update Functions
    void Update()
    {
        DashCounter();
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
                switch (direction)
                {
                    case Direction.right:
                        rb.velocity = Vector2.right * dashSpeed;
                        return;

                    case Direction.left:
                        rb.velocity = Vector2.left * dashSpeed;
                        return;

                    case Direction.up:
                        rb.velocity = Vector2.up * dashSpeed;
                        return;

                    case Direction.down:
                        if(!IsGrounded())
                            rb.velocity = Vector2.down * dashSpeed;
                        return;
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

    #endregion Update Functions
}
