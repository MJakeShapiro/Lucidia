using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoBehaviour
{
    PlayerMovement playerMovement = GameManager.Instance.playerMovement;
    public bool up = false, down = false, left = false, right = false;
    public float resistance = 1.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision!" + collision.name);
        // If the path allows up or down movement
        if (up || down)
        {
            playerMovement.canMoveLeft = false;
            playerMovement.canMoveRight = false;
            // Check if it allows both or just one
            if (up)
                playerMovement.canMoveUp = true;
            else
                playerMovement.canMoveUp = false;
            if (down)
                playerMovement.canMoveDown = true;
            else
                playerMovement.canMoveDown = false;
        }
        // If the path allows left or right movement
        if(left || right)
        {
            playerMovement.canMoveUp = false;
            playerMovement.canMoveDown = false;

            // Check if it allows both or just one
            if (left)
                playerMovement.canMoveLeft = true;
            else
                playerMovement.canMoveLeft = false;
            if (right)
                playerMovement.canMoveRight = true;
            else
                playerMovement.canMoveRight = true;
        }
    }
}
