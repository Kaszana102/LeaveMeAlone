using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aloner : MonoBehaviour
{
    [SerializeField]
    [Range(8,64)]
    int raysNumber = 8;
    float rayLength = 2f;
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

        Vector3 targetPos;
        for(int i = 0; i < raysNumber; i++)
        {
            Ray ray = new Ray(transform.position, Quaternion.Euler(0,360/raysNumber * i,0) * transform.forward );
            RaycastHit hit;

            Vector3 rayOrigin = transform.position;
            Vector3 rayDirection = Quaternion.Euler(0, 360 / raysNumber * i, 0) * transform.forward;

            if (Physics.Raycast(rayOrigin,rayDirection,out hit,rayLength)){
                Debug.DrawRay(rayOrigin, rayDirection, Color.yellow);
            }
            else
            {
                Debug.DrawRay(rayOrigin, rayDirection, Color.yellow);
            }
        }
        
    }    
}
