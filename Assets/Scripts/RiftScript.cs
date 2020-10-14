using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class RiftScript : MonoBehaviour
{
    public AreaEffector2D areaEffector;

    public Direction[] boosted_directions = { Direction.up, Direction.down, Direction.left, Direction.right };


    //Initalized area effector to 0 so player can enter without being shot
    void Start()
    {
        areaEffector = GetComponent<AreaEffector2D>();
        areaEffector.forceMagnitude = 0;
    }

    void Update()
    {
        areaEffector.forceMagnitude = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player myPlayer = other.GetComponent<Player>();

        myPlayer.rift = this;

        if (other.tag == myPlayer.tag && myPlayer.isDashing)
        {
            myPlayer.Launch();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Player myPlayer = other.GetComponent<Player>();

        myPlayer.rift = null;
    }
}
