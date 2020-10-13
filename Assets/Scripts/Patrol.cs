using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed;
    public float distance;
    private bool movingRight = true;
    public Transform wallDetection;
    public Transform groundDetection;
    bool logging = false;
    
    private void Update()
    {
        if (logging)
            Debug.Log("hello");
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D wallInfo = Physics2D.Raycast(wallDetection.position, Vector2.right, distance);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
      
        if (wallInfo.collider.tag == "NPC-Endpoints")
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
        /*else if (groundInfo.collider == false && wallInfo.collider == false)
        {
            transform.Rotate(0,0,-90);
            logging= true;
             /*if(movingRight == true)
              {
                  transform.Rotate(Vector2.down);

                  //transform.eulerAngles = new Vector3(0, -270, 0);
              }
              else
              {
                  transform.Rotate(Vector2.up);
                  //transform.eulerAngles = new Vector3(0, 90, 0);
              }*/
       // }*/
        if (wallInfo.collider.tag != "NPC-Endpoints" && groundInfo.collider == true && wallInfo.collider == true)
        {
            transform.Rotate(0, 0, 90);
        }
        

        /*transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        //if (groundInfo.collider == false)
        //{
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        //}*/
    }
}
