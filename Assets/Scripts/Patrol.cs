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





    [SerializeField] float speed;
    [SerializeField] float wallDistance, groundDistance;
    [SerializeField] Transform rightWallDetection;
    [SerializeField] Transform leftGroundDetection;
    [SerializeField] Transform leftRotationPoint;
    [SerializeField] LayerMask endpoints;


    private void FixedUpdate()
    {
        if(Physics2D.OverlapCircle(rightWallDetection.position, wallDistance, endpoints))
        {
            Debug.Log("TOUCHED AN ENDPOINT");
            transform.Rotate(0, 180, 0);
        }
        if (Physics2D.OverlapCircle(rightWallDetection.position, wallDistance, GameManager.Instance.wall) || Physics2D.OverlapCircle(rightWallDetection.position, wallDistance, GameManager.Instance.ground))
        {
            Debug.Log("TOUCHED WALL OR GROUND ON RIGHT SIDE");
            transform.Rotate(0, 0, 90);
        }

        if(!Physics2D.OverlapCircle(leftGroundDetection.position, groundDistance, GameManager.Instance.ground) && !Physics2D.OverlapCircle(leftGroundDetection.position, groundDistance, GameManager.Instance.wall))
        {
            transform.RotateAround(leftRotationPoint.transform.position, leftRotationPoint.transform.forward, -90);
            Debug.Log("NOT TOUCHING WALL OR GROUND ON LEFT CORNER");
        }

        transform.Translate(Vector2.right * speed * Time.deltaTime);

       

        
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
    }

}
