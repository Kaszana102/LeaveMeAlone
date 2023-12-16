using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aloner : MonoBehaviour
{
    [SerializeField]
    [Range(8,64)]
    int raysNumber = 8;
    [SerializeField]
    float rayLength = 2f,groundRayLength = 1f,
        wallCheckRayLength = 3f;
    [SerializeField]
    [Range(0, 1)]
    float rayAngleOffset = 0.5f;

    [SerializeField] CharacterControler contr;

    GameObject player;
    private Rigidbody2D rb;


    const float stickingRayLength = 0.5f + 0.05f;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();        
    }



    // Update is called once per frame
    void Update()
    {
        //check each ray

        Vector3 targetPos = transform.position;
        Vector3 playerPos = player.transform.position;
        int bestRayIndex = 0;
        int raycastMask = ~LayerMask.GetMask("Aloner");
        for (int i = 0; i < raysNumber; i++)
        {                        
            Vector3 rayOrigin = transform.position;
            Vector3 rayDirection = GetRayDirection(i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, raycastMask);
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

    Vector3 GetRayDirection(int rayIndex)
    {
        return Quaternion.Euler(0, 0, 360 * rayIndex / raysNumber + 360/ raysNumber * rayAngleOffset) * transform.up;
    }

    void DrawRays(int bestRayIndex)
    {
        //draw rays for debug
        for (int i = 0; i < raysNumber; i++)
        {

            Vector3 rayOrigin = transform.position;
            Vector3 rayDirection = GetRayDirection(i);

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
        if (AlonerAboveGround())
        {           

            if (IsWallInFront())
            {
                //move to it
                RunKeepDirection();

                if (StickingToWall())
                {
                    Jump();
                }
            }
            else
            {
                //do the same shit XD
                RunDefaultly(targetPos);
            }
        }
        else
        {            
            RunDefaultly(targetPos);
        }
        

        //check if point is above ground

        if ((PointAboveGround(targetPos) && targetPos.y > transform.position.y) //if need to jump to get to higher place
            ||
            (!AlonerAboveGround() && StickingToWall())) //if in corner
        {                        
            Jump();            
        }        
        


    }

    void RunDefaultly(Vector3 targetPos)
    {
        
        if (targetPos.x > transform.position.x)
        {
            GoRight();
        }
        else
        {
            GoLeft();
        }
        
    }

    void RunKeepDirection()
    {
        if (rb.velocity.x > 0)
        {
            GoRight();
        }
        else
        {
            GoLeft();
        }
    }

    bool AlonerAboveGround()
    {
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = Vector3.down;
        int raycastMask = ~(LayerMask.GetMask("Aloner") + LayerMask.GetMask("Player"));

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, stickingRayLength, raycastMask);
        return hit.collider == null;
    }

    bool IsWallInFront()
    {
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = Vector3.right;
        int raycastMask = ~(LayerMask.GetMask("Aloner") + LayerMask.GetMask("Player"));
        if (rb.velocity.x < 0)
        {
            rayDirection *= -1;
        }
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection,wallCheckRayLength, raycastMask);
        Debug.DrawRay(rayOrigin, rayDirection * wallCheckRayLength, Color.black);

        return hit.collider != null;        
    }

    bool StickingToWall()
    {
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = Vector3.right;
        int raycastMask = ~(LayerMask.GetMask("Aloner") + LayerMask.GetMask("Player"));

        RaycastHit2D hitRight = Physics2D.Raycast(rayOrigin, rayDirection, stickingRayLength, raycastMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(rayOrigin, -rayDirection, stickingRayLength, raycastMask);

        return hitRight.collider != null || hitLeft.collider !=null;
    }

    void GoRight()
    {
        contr.Run(true);        
    }

    void GoLeft()
    {
        contr.Run(false);        
    }

    void Jump()
    {
        contr.Jump();        
    }


    bool PointAboveGround(Vector3 point)
    {        
        Vector3 rayOrigin = point;
        Vector3 rayDirection = Vector3.down;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, groundRayLength);
        //check if ray collided with anything
        if (hit.collider != null)
        {   //hit anything

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
