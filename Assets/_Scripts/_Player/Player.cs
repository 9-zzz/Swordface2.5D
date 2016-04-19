using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public static Player S;

    public float moveSpeed = 6.0f;//6;       // I made this exposed. ###
    public float dashSpeed = 12.0f;//6;       // I made this exposed. ###

    //public float jumpHeight = 3.5f;//3.5f; // E10 Jump logic and equation! ###

    public float maxJumpHeight = 3.5f;//3.5f;
    public float minJumpHeight = 1.0f;//1.0f;
    public float timeToJumpApex = 0.4f;//.4f;

    public int airJumpsAllowed = 1; // set to 0 if can only jump when on ground.
	int currentAirJumpCount;

    public int jumpCounter = 0;
    public int baseNumberOfJumps = 0;

    float accelerationTimeAirborne = 0.2f;//.2f;
    float accelerationTimeGrounded = 0.1f;//.1f;

    public Vector2 wallJumpClimb; // (7.5, 16)
    public Vector2 wallJumpOff;   // (8.5, 7)
    public Vector2 wallLeap;      // (18,  17)

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f; // Fixes issue of tricky 'wallLeap' since once you start move opposite you also quickly start moving down.
    float timeToWallUnstick;

    float gravity;

    //float maxJumpVelocity; // E10 Jump logic and equation! ###

    float maxJumpVelocity;
    float minJumpVelocity;

    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

    //public float playerXspeed;

    void Awake()
    {
        S = this;
    }

    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight); // Kinematic solved for minJumpVelocity.
        print("Gravity: " + gravity + "  Jump Velocity: " + maxJumpVelocity);

        //print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity); // Old code before variable jump height E10.
    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // Moved to top, wall jumping code stuff.
        int wallDirX = (controller.collisions.left) ? -1 : 1; // This is going to be -1 if collide wall to left of us, and positive 1 if collide wall to right of us.

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);


                //playerXspeed = velocity.x;
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

        //if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below) // No longer the case with wall jumping code in.

        //if (Input.GetKeyDown(KeyCode.Space))
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
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

            //if (controller.collisions.below) // Regular jump.
            if (controller.collisions.below || currentAirJumpCount < airJumpsAllowed)
            {
                velocity.y = maxJumpVelocity;

                // Double jump code begin
                if (!controller.collisions.below)
                {
                    currentAirJumpCount++;
                }
                else {
                    currentAirJumpCount = 0;
                }
                // Double jump code end
            }

        } // END OF Jumping code + wall jump.

        //if (Input.GetKeyUp(KeyCode.Space)) // For variable jump height E10 @2:25 code.
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
        }

        // <-- Wall Jumping Code
        // velocity.x ... was here... moved for wall jump code.
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, input);

        // E10 @11:55 Explains why this was moved from above Space input to below .Move call.
        // Moving platform ALSO CALLS .Move().
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        // Change face code
        if (input.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(input.x), 1, 1);
            //transform.localScale = new Vector3(controller.facing, 1, 1);
        }
        //NOTE: using controller.facing instead of sign(input.x) will result in player flipping dir while walljumping
        // If this is desirable behaviour, use the following line instead:
        // Change face code

        // Dashing Code
        //print(velocity.x + " and " + targetVelocityX); //print(Mathf.Sign(input.x));
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            velocity.x = controller.facing * dashSpeed;
            GlitchHandler.S.ColorDriftFXMethod();
            GlitchHandler.S.ScanLineFXMethod();
            transform.GetChild(2).GetComponent<ParticleSystem>().Play();
            //velocity.x = Mathf.Sign(input.x) * dashSpeed;
        }

    } // END OF Update() METHOD

    public void ExternalJump(float js)
    {
        velocity.y = js;
    }

}
