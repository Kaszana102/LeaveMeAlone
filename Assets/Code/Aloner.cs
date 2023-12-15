using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aloner : MonoBehaviour
{
    [SerializeField]
    [Range(8,64)]
    int raysNumber = 8;
    [SerializeField]
    float rayLength = 2f;
    [SerializeField]
    float groundRayLength = 1f;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //check each ray

        Vector3 targetPos = transform.position;
        Vector3 playerPos = player.transform.position;
        int bestRayIndex = 0;
        for (int i = 0; i < raysNumber; i++)
        {                        
            Vector3 rayOrigin = transform.position;
            Vector3 rayDirection = Quaternion.Euler(0, 0, 360 * i / raysNumber) * transform.up;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength);
            //check if ray collided with anything
            if (hit.collider != null)
            {
                //if hit anything
                if (Vector3.Distance(playerPos, targetPos) < Vector3.Distance(playerPos, hit.point))
                {
                    targetPos = hit.point;
                    bestRayIndex = i;
                }
            }
            else
            {   // missed        

                if (Vector3.Distance(playerPos, targetPos) < Vector3.Distance(playerPos, rayOrigin + rayDirection * rayLength))
                {
                    targetPos = rayOrigin + rayDirection * rayLength;
                    bestRayIndex = i;
                }
            }
        }

        DrawRays(bestRayIndex);



        ChooseAction(targetPos);

    }    

    void DrawRays(int bestRayIndex)
    {
        //draw rays for debug
        for (int i = 0; i < raysNumber; i++)
        {

            Vector3 rayOrigin = transform.position;
            Vector3 rayDirection = Quaternion.Euler(0,0,360 * i / raysNumber ) * transform.up;

            if (i == bestRayIndex)
            {
                Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.red);
            }
            else
            {
                Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.yellow);
            }
        }
    }
    
    void ChooseAction(Vector3 targetPos)
    {
        if (targetPos.x > transform.position.x)
        {
            GoRight();
        }
        else
        {
            GoLeft();
        }

        //check if point is above ground
        if (PointAboveGround(targetPos))
        {
            if (targetPos.y > transform.position.y)
            {
                Jump();
            }
        }


    }

    void GoRight()
    {
        Debug.Log("RIGHT");
    }

    void GoLeft()
    {
        Debug.Log("LEFT");
    }

    void Jump()
    {
        Debug.Log("JUMP");
    }


    bool PointAboveGround(Vector3 point)
    {
        

        Vector3 rayOrigin = point;
        Vector3 rayDirection = -transform.up;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, groundRayLength);
        //check if ray collided with anything
        if (hit.collider != null)
        {
            //if hit anything

            Debug.DrawRay(rayOrigin, rayDirection * groundRayLength, Color.green);

            return true;
            
        }
        else
        {   // missed        
            Debug.DrawRay(rayOrigin, rayDirection * groundRayLength, Color.white);
            return false;
        }
    }
}
