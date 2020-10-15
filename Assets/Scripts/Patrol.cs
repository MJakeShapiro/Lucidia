using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float moveSpeed;
    public GameObject[] rotationPoints;
    int nestRotationPoint = 0;
    float distToPoint;
    bool back = false;
    void Update()
    {
        Move();
    }    

    void Move()
    {
        distToPoint = Vector2.Distance(transform.position, rotationPoints[nestRotationPoint].transform.position);
        transform.position = Vector2.MoveTowards(transform.position, rotationPoints[nestRotationPoint].transform.position, moveSpeed * Time.deltaTime);
        
        if(distToPoint < 0.2f)
        {
            TakeTurn();
        }
    }
    
    void TakeTurn()
    {
        Vector3 currentRot = transform.eulerAngles;
        currentRot.z += rotationPoints[nestRotationPoint].transform.eulerAngles.z;
        transform.eulerAngles = currentRot;
        pickNextPoint();
    }

    void pickNextPoint()
    {
        nestRotationPoint++;

        if(nestRotationPoint == rotationPoints.Length)
        {
            nestRotationPoint = 0;
        }
    }

    /*
    public Transform mytransform;
    public float speed = 5f
    public bool isWalking = true;
    public Vector3 objNorm = Vector3.up;
    public Vector3 objNorm = Vector3.zero;

    private void Start()
    {
        mytransform = transform;
    }

    private void Update()
    {
        switch(isWalking)
        {
            case true:
                RaycastHit rayHit;
                if(Physics.Raycast(mytransform.position, mytransform.forward, rayHit, 0.5f))
                {
                    hitNormal = rayHit.normal;
                    isWalking = false;
                }
                Vector3 lookBehind = mytransform.position + (-mytransform.forward * 0.25f);
                if(Physics.Raycast(lookBehind, -mytransform.up, ))
        }
    }
    */




    /*
    public float speed;
    public float distance;
    private bool movingRight = true;
    public Transform wallDetection;
    public Transform groundDetection;
 
    
    private void Update()
    {
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
        else if (groundInfo.collider == false && wallInfo.collider == false)
        {
            transform.Rotate(0,0,-90);
             /*//*if(movingRight == true)
              {
                  transform.Rotate(Vector2.down);

                  //transform.eulerAngles = new Vector3(0, -270, 0);
              }
              else
              {
                  transform.Rotate(Vector2.up);
                  //transform.eulerAngles = new Vector3(0, 90, 0);
              }*/
    /*
        }
        if (wallInfo.collider.tag != "NPC-Endpoints" && groundInfo.collider == true && wallInfo.collider == true)
        {
            transform.Rotate(0, 0, 90);
        }
    }
        */
}
