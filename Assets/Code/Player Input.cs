using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] CharacterControler contr;

    // Update is called once per frame
    void Update()
    {
        //Running
        if (Input.GetButton("Left"))
            contr.Run(false);
        else if (Input.GetButton("Right"))
            contr.Run(true);
        else
            contr.Stop();

        //Jumping
        if (Input.GetButtonDown("Jump"))
            contr.Jump();
        if (!Input.GetButton("Jump"))
            contr.StopJump();
    }
}
