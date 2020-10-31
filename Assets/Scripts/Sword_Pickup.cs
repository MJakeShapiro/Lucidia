using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Pickup : MonoBehaviour
{
    public Player player_script;
    // Start is called before the first frame update
   
    private void OnTriggerEnter2D(Collider2D test)
    {
        player_script.sword_sprite.SetActive(true);
        OnDestroy();

    }
    public void OnDestroy()
    {
        Destroy(gameObject);
    }
    
}
