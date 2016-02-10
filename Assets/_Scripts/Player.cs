using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public static Player S;

    public float moveSpeed = 6.0f;//6;
    public float jumpHeight = 3.5f;//3.5f;
    public float timeToJumpApex = 0.4f;//.4f;
    float accelerationTimeAirborne = 0.2f;//.2f;
    float accelerationTimeGrounded = 0.1f;//.1f;

    public Vector2 wallJumpClimb; // (7.5, 16)
    public Vector2 wallJumpOff;   // (8.5, 7)
    public Vector2 wallLeap;      // (18,  17)

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f; // Fixes issue of tricky 'wallLeap' since once you start move opposite you also quickly start moving down.
    float timeToWallUnstick;

    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

    void Awake()
    {
        S = this;
    }

    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // Moved to top, wall jumping code stuff.
        int wallDirX = (controller.collisions.left) ? -1 : 1; // This is going to be -1 if collide wall to left of us, and positive 1 if collide wall to right of us.

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        // Wall Jumping Code -->

        // Check for the case where this is true. 
        // Need to be colliding with a wall to the left or right.
        // Needs to not be touching the ground and Also has to be moving downwards.
        bool wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;  // Weird results if we don't rest used with 'ref' above in smoothdampf line.
                velocity.x = 0;          // While we want to remain stuck the wall. This is why input.x and SmoothDampf were moved up before this.

                if (input.x != wallDirX && input.x != 0) // We are moving away from wall we're hitting, moving opposite for 'wallLeap'.
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime; // Reset
                }
            }
            else // If none of above is true also just reset. fix?
            {
                timeToWallUnstick = wallStickTime;
            }

        }

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        //if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below) // No longer the case with wall jumping code in.

        //if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallSliding)
            {
                if (wallDirX == input.x)                        // Wall jump and moving into direction of wall.
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (input.x == 0)                          // Where we just jump off the wall. On wall. 
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else                                            // When we have an input that is opposite to wall direction.
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            // No longer wall sliding here

            if (controller.collisions.below) // Regular jump.
            {
                velocity.y = jumpVelocity;
            }

        } // END OF Jumping code + wall jump.

        // <-- Wall Jumping Code
        // velocity.x ... was here... moved for wall jump code.
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}
