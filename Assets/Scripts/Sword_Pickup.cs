using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Pickup : MonoBehaviour
{
    // Start is called before the first frame update
    public Collider2D sword;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        sword.isTrigger = false;
        if (sword == true)
            OnTriggerEnter2D(sword);
        else
            OnTriggerExit2D(sword);
            
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
            Debug.Log("Sword is picked up");
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Sword is picked down");
    }
}
