using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    [SerializeField] private float 
        runGroundAcc, runGroundDecc, runGroundTop,  //acceleration, decceleration and top speed on ground
        runAirAcc, runAirDecc, runAirTop,           //acceleration, decceleration and top speed in air
        vertTop,                                    //top vertical speed
        jumpForce, peakVertSpeed,                   //upwards force of jump and definition of jump peak
        wallJmpUp, wallJmpHor, wallCling,           //Vertical and Horizontal force of wall jump and force of sticking to walls
        groundGrav, upGrav, stopGrav, peakGrav,     //gravity on ground, when jumping, when stopping jump and during jump peak
        fallGrav, wallUpGrav, wallDownGrav,         //gravity when falling and wall sliding up or down
        coyoteTime, jumpBufferTime, wallCtrlLoss;   //times of coyote time, jump buffering and lost control after wall jump

    private Rigidbody2D rb;
    private CharacterAudio audioSource;

    float 
        _coyoteTimer, _jumpBufferTimer, _ctrlTimer,
        floorCheckLength = 0.01f, collRadius = 0.5f;

    int wallOnRight;

    public enum States
    {
        ground,
        jumping,
        falling,
        wall
    };

    States state = States.ground;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<CharacterAudio>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Floor check, Raycast is ugly but works (for now)
        RaycastHit2D hitFloor = Physics2D.Raycast(rb.position + Vector2.down * (collRadius + 0.001f),
            Vector2.down, floorCheckLength);
        if (state != States.ground && state != States.jumping && hitFloor)
        {
            state = States.ground;
            audioSource.PlayLandingSound();
        }
        else if (state == States.ground && !hitFloor)
        {
            state = States.falling;
            _coyoteTimer = coyoteTime;
        }

        //Wall check
        if (state != States.ground && _ctrlTimer <= 0)
        {
            RaycastHit2D hitWallRight = Physics2D.Raycast(rb.position + Vector2.right * (collRadius + 0.001f),
                Vector2.right, floorCheckLength);
            RaycastHit2D hitWallLeft = Physics2D.Raycast(rb.position + Vector2.left * (collRadius + 0.001f),
                Vector2.left, floorCheckLength);
            if (hitWallLeft)
            {
                state = States.wall;
                wallOnRight = 1;
            }
            else if (hitWallRight)
            {
                state = States.wall;
                wallOnRight = -1;
            }
            else if (state == States.wall)
                state = States.falling;

        }

        //Limit speed to Top
        if (state == States.ground && Mathf.Abs(rb.velocity.x) > runGroundTop)
            rb.velocity = new Vector2(runGroundTop * Mathf.Sign(rb.velocity.x), rb.velocity.y);
        else if (Mathf.Abs(rb.velocity.x) > runAirTop)
            rb.velocity = new Vector2(runAirTop * Mathf.Sign(rb.velocity.x), rb.velocity.y);
        if (Mathf.Abs(rb.velocity.y) > vertTop)
            rb.velocity = new Vector2(rb.velocity.x, vertTop * Mathf.Sign(rb.velocity.y));

        //Set gravity
        if (state == States.ground)
            rb.gravityScale = groundGrav;
        else
        {
            if (rb.velocity.y < 0 && state != States.wall)
            {
                rb.gravityScale = fallGrav;
                state = States.falling;
            }
            else
            {
                if (state == States.falling)
                    rb.gravityScale = stopGrav;
                else if (state == States.wall)
                {
                    rb.AddForce(Vector2.right * wallOnRight * wallCling);
                    if (rb.velocity.y <= 0)
                        rb.gravityScale = wallDownGrav;
                    else
                        rb.gravityScale = wallUpGrav;
                }
                else if (rb.velocity.y <= peakVertSpeed)
                    rb.gravityScale = peakGrav;
                else
                    rb.gravityScale = upGrav;
            }
        }
    }

    private void Update()
    {
        //Update timer
        _coyoteTimer -= Time.deltaTime;
        _jumpBufferTimer -= Time.deltaTime;
        _ctrlTimer -= Time.deltaTime;


        //Buffered inputs
        if (_jumpBufferTimer > 0 && state == States.ground)
            Jump();
    }

    //Left and right movement
    public void Run(bool right)
    {
        if (_ctrlTimer <= 0)
        {
            int dir = 1;
            if (!right)
                dir = -1;

            if (state == States.ground)
            {
                audioSource.PlayRunSound();
                if (right == (rb.velocity.x > 0))
                    rb.AddForce(Vector2.right * dir * runGroundAcc);
                else
                    rb.AddForce(Vector2.right * dir * runGroundDecc);
            }
            else
            {
                if (right == (rb.velocity.x > 0))
                    rb.AddForce(Vector2.right * dir * runAirAcc);
                else
                    rb.AddForce(Vector2.right * dir * runAirDecc);
            }
        }
    }

    public void Stop()
    {
        if (_ctrlTimer <= 0)
        {
            if (Mathf.Abs(rb.velocity.x) < 0.05f)
                rb.velocity = new Vector2(0, rb.velocity.y);

            else if (state == States.ground)
                rb.AddForce(Vector2.right * -Mathf.Sign(rb.velocity.x) * Mathf.Min(runGroundDecc, Mathf.Abs(rb.velocity.x)));
            else
            {
                rb.AddForce(Vector2.right * -Mathf.Sign(rb.velocity.x) * Mathf.Min(runAirDecc, Mathf.Abs(rb.velocity.x)));
            }
        }
    }

    public void Jump()
    {
        if (_ctrlTimer <= 0)
        {
            if (state != States.ground && state != States.wall && _coyoteTimer <= 0)
                _jumpBufferTimer = jumpBufferTime;
            else if (state == States.wall)
            {
                audioSource.PlayJumpSound();
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * wallJmpUp + Vector2.right * wallOnRight * wallJmpHor, ForceMode2D.Impulse);
                _ctrlTimer = wallCtrlLoss;
                state = States.jumping;
            }
            else
            {
                audioSource.PlayJumpSound();
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                _coyoteTimer = 0;
                _jumpBufferTimer = 0;
                state = States.jumping;
            }
        }
    }

    public void StopJump()
    {
        if (_ctrlTimer <= 0)
        {
            if (state != States.ground && state != States.wall)
                state = States.falling;
        }
    }

    public States GetState() => state;

    public int IsWallOnRight() => wallOnRight;
}
