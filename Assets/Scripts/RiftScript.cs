using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftScript : MonoBehaviour
{
    public AreaEffector2D areaEffector;


    //Initalized area effector to 0 so player can enter without being shot
    void Start()
    {
        areaEffector = GetComponent<AreaEffector2D>();
        areaEffector.forceMagnitude = 0;
    }

    void Update()
    {
        areaEffector.forceMagnitude = 0;
        areaEffector.forceVariation = 0;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Player myPlayer = other.GetComponent<Player>();
        if (myPlayer.isDashing == true)
        {
            areaEffector.forceMagnitude = 400;
            areaEffector.forceVariation = 100;
            Debug.Log("Rift Triggered");
        }

        Debug.Log("Rift Not Triggered");
    }
}
