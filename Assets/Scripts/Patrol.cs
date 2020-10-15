using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Patrol : MonoBehaviour
{
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
            transform.Rotate(0, 180, 0);
        }
        if (Physics2D.OverlapCircle(rightWallDetection.position, wallDistance, GameManager.Instance.wall) || Physics2D.OverlapCircle(rightWallDetection.position, wallDistance, GameManager.Instance.ground))
        {
            transform.Rotate(0, 0, 90);
        }

        if(!Physics2D.OverlapCircle(leftGroundDetection.position, groundDistance, GameManager.Instance.ground) && !Physics2D.OverlapCircle(leftGroundDetection.position, groundDistance, GameManager.Instance.wall))
        {
            transform.RotateAround(leftRotationPoint.transform.position, leftRotationPoint.transform.forward, -90);
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

}
