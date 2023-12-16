using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CharacterGraphics : MonoBehaviour
{
    [SerializeField] CharacterControler contr;
    [SerializeField] SpriteShapeController shape;
    [SerializeField] Transform graphics;
    [SerializeField] float displStick, displMove, topSpeed;
    Spline spline;      //0-left 1-top 2-right 3-bottom
    CharacterControler.States state;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        spline = shape.spline;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        state = contr.GetState();
        ResetSpline();
        graphics.rotation = Quaternion.identity;

        if (state == CharacterControler.States.ground)
        {
            spline.SetPosition(0, spline.GetPosition(0) + (Vector3.down + Vector3.left * 0.5f) * displStick);
            spline.SetPosition(2, spline.GetPosition(2) + (Vector3.down + Vector3.right * 0.5f) * displStick);
        }
        else if (state == CharacterControler.States.wall)
        {
            spline.SetPosition(1, spline.GetPosition(1) + (Vector3.right + Vector3.up * 0.5f) * -contr.IsWallOnRight() * displStick);
            spline.SetPosition(3, spline.GetPosition(3) + (Vector3.right + Vector3.down * 0.5f) * -contr.IsWallOnRight() * displStick);
        }
        else
        {
            graphics.up = rb.velocity.normalized;
            spline.SetPosition(3, spline.GetPosition(3) + 
                Vector3.down * rb.velocity.sqrMagnitude / (topSpeed * topSpeed) * displMove);
        }
    }


    void ResetSpline()
    {
        spline.SetPosition(0, new Vector3(-1, 0, 0));
        spline.SetPosition(1, new Vector3(0, 1, 0));
        spline.SetPosition(2, new Vector3(1, 0, 0));
        spline.SetPosition(3, new Vector3(0, -1, 0));
    }
}
