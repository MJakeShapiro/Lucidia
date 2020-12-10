using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;

public enum type
{
    isAll, isUp, isDown, isLeft, isRight, isLeftRight, isUpDown
}

public class RiftScript : MonoBehaviour
{
    public AreaEffector2D areaEffector;

    [HideInInspector] public Direction[] boosted_directions; // = { Direction.up, Direction.down, Direction.left, Direction.right };

    public type choice;

    public Animator animator;

    bool arrow;

    bool blank;


    //Initalized area effector to 0 so player can enter without being shot
    void Start()
    {
        switch (choice)
        {
            case type.isAll:
                animator.SetBool("isAll", true);
                boosted_directions = new Direction[] { Direction.up, Direction.down, Direction.left, Direction.right };
                break;
            case type.isUp:
                animator.SetBool("isUp", true);
                boosted_directions = new Direction[] { Direction.up };
                break;
            case type.isDown:
                animator.SetBool("isDown", true);
                boosted_directions = new Direction[] { Direction.down };
                break;
            case type.isLeft:
                animator.SetBool("isLeft", true);
                boosted_directions = new Direction[] { Direction.left };
                break;
            case type.isRight:
                animator.SetBool("isRight", true);
                boosted_directions = new Direction[] { Direction.right };
                break;
            case type.isLeftRight:
                animator.SetBool("isLeftRight", true);
                boosted_directions = new Direction[] { Direction.left, Direction.right };
                break;
            case type.isUpDown:
                animator.SetBool("isUpDown", true);
                boosted_directions = new Direction[] { Direction.up, Direction.down };
                break;
        }

        arrow = false;
        blank = true;

        areaEffector = GetComponent<AreaEffector2D>();
        areaEffector.forceMagnitude = 0;
    }

    void Update()
    {
        areaEffector.forceMagnitude = 0;
    }

    void FixedUpdate()
    {
        if (blank)
        {
            blank = false;
            animator.SetBool("blank", blank);
            arrow = true;
            animator.SetBool("arrow", arrow);
        }
        else
        {
            arrow = false;
            //animator.SetBool("arrow", arrow);
            blank = true;
            //animator.SetBool("blank", blank);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player myPlayer = other.GetComponent<Player>();

        myPlayer.rift = this;
        myPlayer.inRift = true;

        if (other.tag == myPlayer.tag && myPlayer.isDashing)
        {
            for (int i = 0; i < boosted_directions.Length; i++)
            {
                if (myPlayer.GetDirection == boosted_directions[i])
                {
                    myPlayer.boosted = true;
                    myPlayer.Launch();
                }
            }

        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Player myPlayer = other.GetComponent<Player>();

        myPlayer.rift = this;

        if (other.tag == myPlayer.tag && myPlayer.isDashing)
        {
            for (int i = 0; i < boosted_directions.Length; i++)
            {
                if (myPlayer.GetDirection == boosted_directions[i])
                {
                    myPlayer.boosted = true;
                    myPlayer.Launch();
                }
            }

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Player myPlayer = other.GetComponent<Player>();
        myPlayer.inRift = false;
        myPlayer.rift = null;
    }
}
