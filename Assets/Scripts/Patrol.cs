using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    //public float moveSpeed;
    //public GameObject[] rotationPoints;
    //int nestRotationPoint = 0;
    //float distToPoint;
    //bool back = false;
    //void Update()
    //{
    //    Move();
    //}    

    //void Move()
    //{
    //    distToPoint = Vector2.Distance(transform.position, rotationPoints[nestRotationPoint].transform.position);
    //    transform.position = Vector2.MoveTowards(transform.position, rotationPoints[nestRotationPoint].transform.position, moveSpeed * Time.deltaTime);
        
    //    if(distToPoint < 0.2f)
    //    {
    //        TakeTurn();
    //    }
    //}
    
    //void TakeTurn()
    //{
    //    Vector3 currentRot = transform.eulerAngles;
    //    currentRot.z += rotationPoints[nestRotationPoint].transform.eulerAngles.z;
    //    transform.eulerAngles = currentRot;
    //    pickNextPoint();
    //}

    //void pickNextPoint()
    //{
    //    nestRotationPoint++;

    //    if(nestRotationPoint == rotationPoints.Length)
    //    {
    //        nestRotationPoint = 0;
    //    }
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    for (int i = 0; i < rotationPoints.Length; i++)
    //    {
    //        Gizmos.DrawWireCube(rotationPoints[i].transform.position, new Vector3(1.0f, 1.0f, 0.0f));
    //    }
    //}
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




    
    public float speed;
    public float wallDistance, groundDistance;
    private bool movingRight = true;
    private bool passedRight = false, passedLeft = false;
    public Transform rightWallDetection, rightGroundDetection;
    public Transform leftWallDetection,  leftGroundDetection;
    public Transform leftRotationPoint, rightRotationPoint;
    RaycastHit2D rightWallInfo, leftWallInfo, rightGroundInfo, leftGroundInfo;


    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D rightWallInfo = Physics2D.Raycast(rightWallDetection.position, Vector2.right, wallDistance);
        RaycastHit2D rightGroundInfo = Physics2D.Raycast(rightGroundDetection.position, Vector2.down, groundDistance);
        RaycastHit2D leftGroundInfo = Physics2D.Raycast(leftGroundDetection.position, Vector2.down, groundDistance);
        RaycastHit2D leftWallInfo = Physics2D.Raycast(leftWallDetection.position, Vector2.left, wallDistance);

        // Check if meeting edge on right side to wrap around
        if (rightGroundInfo)
        {
            if (rightGroundInfo.transform.gameObject.layer != LayerMask.NameToLayer("Ground") && rightGroundInfo.transform.gameObject.layer != LayerMask.NameToLayer("Wall"))
            {
                if (passedLeft)
                {
                    transform.Rotate(0, 0, -270);
                    passedLeft = false;
                }
                else
                    passedRight = true;

                //transform.Rotate(0, 0, -90);
            }
        }
        else
        {
            if (passedLeft)
            {
                transform.Rotate(0, 0, -270);
                passedLeft = false;
            }

            else
                passedRight = true;
            //transform.Rotate(0, 0, 90);
        }

        // Check if meeting edge on left side to wrap around
        if (leftGroundInfo)
        {
            if (leftGroundInfo.transform.gameObject.layer != LayerMask.NameToLayer("Ground") && leftGroundInfo.transform.gameObject.layer != LayerMask.NameToLayer("Wall"))
            {
                if (passedRight)
                {
                    transform.RotateAround(leftRotationPoint.transform.position, leftRotationPoint.transform.forward, -90);
                    passedRight = false;
                }
                else
                    passedLeft = true;
            }
        }
        else
        {
            Debug.LogError("leftGround2");

            if (passedRight)
            {
                transform.RotateAround(leftRotationPoint.transform.position, leftRotationPoint.transform.forward, -90);
                passedRight = false;
            }

            else
                passedLeft = true;

        }

        // Check if meeting wall or floor corner on right side
        if (rightWallInfo)
        {
            if (rightWallInfo.collider.tag == "NPC-Endpoints")
            {
                Debug.LogError("right ENDPOINT");
                transform.localEulerAngles = new Vector3(0, 180, 0);
                Debug.LogError(leftGroundInfo);

            }
            if (rightWallInfo.transform.gameObject.layer == LayerMask.NameToLayer("Ground") || rightWallInfo.transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Debug.Log("right Wall or Ground");
                transform.Rotate(0, 0, 90);

            }
        }

        // Check if meeting wall or floor corner on right side
        if (leftWallInfo)
        {
            if (leftWallInfo.collider.tag == "NPC-Endpoints")
            {
                Debug.LogError("left ENDPOINT");
                transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            if (leftWallInfo.transform.gameObject.layer == LayerMask.NameToLayer("Ground") || leftWallInfo.transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Debug.Log("left Wall or Ground");
                transform.Rotate(0, 0, -90);

            }
        }
        //if (wallInfo.collider.tag == "NPC-Endpoints")
        //{
        //    if (movingRight == true)
        //    {
        //        transform.eulerAngles = new Vector3(0, -180, 0);
        //        movingRight = false;
        //    }
        //    else
        //    {
        //        transform.eulerAngles = new Vector3(0, 0, 0);
        //        movingRight = true;
        //    }
        //}
        //else if (groundInfo.collider == false && wallInfo.collider == false)
        //{
        //    transform.Rotate(0, 0, -90);
        //    if (movingRight == true)
        //    {
        //        transform.Rotate(Vector2.down);

        //        transform.eulerAngles = new Vector3(0, -270, 0);
        //    }
        //    else
        //    {
        //        transform.Rotate(Vector2.up);
        //        transform.eulerAngles = new Vector3(0, 90, 0);
        //    }
        //}
        //if (wallInfo.collider.tag != "NPC-Endpoints" && groundInfo.collider == true && wallInfo.collider == true)
        //{
        //    transform.Rotate(0, 0, 90);
        //}
    }

}
