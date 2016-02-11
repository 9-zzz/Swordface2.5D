using UnityEngine;
using System.Collections;

public class Controller2D : RaycastController 
{
    public static Controller2D S;

    float maxClimbAngle = 80;
    float maxDescendAngle = 80;
    public int facing;

    public CollisionInfo collisions;
    public Vector2 playerInput;

    void Awake()
    {
        S = this;
    }

    // ??? ->RaycastController start() is virtual... @5:00 in E06 Tutorial Video SL ######
    public override void Start()
    {
        // So we can have a start in 'RaycastController' and any extended script.
        base.Start();

        collisions.faceDir = 1;
    }

    // An OVERLOAD method for the 'input' param in Move() below.
    public void Move(Vector3 velocity, bool standingOnPlatform)
    {
        Move(velocity, Vector2.zero, standingOnPlatform); // Pass in zero for input. Now moving platform class doesn't have to worry.
    }

    public void Move(Vector3 velocity, Vector2 input, bool standingOnPlatform = false) // Also being called from the platform mover so why have 'input'? E10 6:50.
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.velocityOld = velocity;
        playerInput = input;

        if (velocity.x != 0)
        {
            collisions.faceDir = (int)Mathf.Sign(velocity.x); // For faceDir and wall jumping code.
        }

        if (velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }

        /* Wall Jumping Code
        if (velocity.x != 0) // So now when you're just standing next to a wall... ??? do i want this feature? @3:17 E09 ######
        {
            HorizontalCollisions(ref velocity); // copied this out
        }
        // Wall Jumping Code */
        HorizontalCollisions(ref velocity);

        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);

        if (standingOnPlatform)
            collisions.below = true; // Fixes no jump on platform moving up!

        // Flip way player faces...
        facing = collisions.faceDir;
        //transform.localScale = new Vector3(facing, 1, 1);
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        //float directionX = Mathf.Sign(velocity.x); // Facedir and wall jumping code. already doing sign math above. @4:20 in E09.
        float directionX = collisions.faceDir;
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        // So if while next to a wall not pressing anything it does a slow down wall jump.
        if (Mathf.Abs(velocity.x) < skinWidth)
        {
            rayLength = 2 * skinWidth; // Moving ray to edge of collider and 2 times for ray length to detect wall. 
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                // "Skip ahead to next ray and use that for collisions." E07 @13:30 This solves moving side to side while being crushed from above by moving platform.
                if (hit.distance == 0)
                {
                    continue;
                }

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        velocity = collisions.velocityOld;
                    }
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }

                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if (collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                // Falling through platforms and moving platforms CODE 'VerticalCollisions'.
                if (hit.collider.tag == "Through")
                {
                    if (directionY == 1 || hit.distance == 0) // If we're moving up.
                    {
                        continue; // Go to next RAY in THIS FOR-LOOP... ???
                    }
                    if (collisions.fallingThroughPlatform)
                    {
                        continue;
                    }
                    if (playerInput.y == -1) // E10 @10:02 Makes it so when you press DOWN it holds it for a bit 'Invoke(.5f)' more! ###
                    {
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", .5f); // Reset this bool after a half a second.
                        continue;
                    }
                }
                // Falling through platforms and moving platforms CODE 'VerticalCollisions'.

                velocity.y = (hit.distance - skinWidth) * directionY; // Where we actually constrain velocity to stop from moving through things.
                rayLength = hit.distance;

                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }

        if (collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle)                    // We have collided with a new slope, multiply by nothing.
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;                     // Fixes E05 glitch slope corner??
                }
            }
        }
    }

    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    void ResetFallingThroughPlatform()
    {
        collisions.fallingThroughPlatform = false;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector3 velocityOld;
        public int faceDir;                     // 1 = facing right, -1 = facing left.
        public bool fallingThroughPlatform;     // E10 @10:02 Makes it so when you press DOWN it hold it for a bit more! ###

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

}
