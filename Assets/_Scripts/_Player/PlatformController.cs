using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For hash sets. movedPassengers, fast at adding things and checking if they contain a set of things.

public class PlatformController : RaycastController
{

    public LayerMask passengerMask;

    //
    public Color gizmoColor = Color.red; // *
    public Vector3[] localWaypoints;
	Vector3[] globalWaypoints;

    public float speed = 5.0f;
	public bool cyclic;
	public float waitTime = 1.0f;

	[Range(0,2)] // Actually 1-3
	public float easeAmount = 1.0f;

    int fromWaypointIndex;
	float percentBetweenWaypoints; // Between zero and one ...  0 & 1
	float nextMoveTime;
    //

    List<PassengerMovement> passengerMovement; // To store all in.
	Dictionary<Transform,Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D>(); // For reducing GetComponent calls optimization.

    public override void Start()
    {
        base.Start();

        globalWaypoints = new Vector3[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position; // At start of game for points.
        }
    }

    void Update()
    {
        UpdateRaycastOrigins();

        Vector3 velocity = CalculatePlatformMovement(); // Set to the result of that method...

        CalculatePassengerMovement(velocity);

        //Vector3 velocity = move * Time.deltaTime; // Old move

        CalculatePassengerMovement(velocity);

        MovePassengers(true);
        transform.Translate(velocity);
        MovePassengers(false);
    }

    // @14:08 E08 ########### Explained math formula for easing in update w/ out choroutine.
    float Ease(float x)
    {
        float a = easeAmount + 1; // When ZERO our a-value is 1, so above range is 1-3
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    Vector3 CalculatePlatformMovement()
    {

        if (Time.time < nextMoveTime)
        {
            return Vector3.zero;
        }

        fromWaypointIndex %= globalWaypoints.Length;
        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints; // Fixes the more further WPs are the faster it might move. divide by dist.
        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);             // Clamp zero one...
        float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

        if (percentBetweenWaypoints >= 1)
        {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;

            // If not cyclic then move back one, once you hit the end of WPs instead of going directly from end to beginning.
            if (!cyclic)
            {
                if (fromWaypointIndex >= globalWaypoints.Length - 1)
                {
                    fromWaypointIndex = 0;                            // So it starts again at beginning of WPs
                    System.Array.Reverse(globalWaypoints);
                }
            }
            nextMoveTime = Time.time + waitTime;
        }

        return newPos - transform.position; // It sends this every frame.

    } // END OF CalculatePlatformMovement METHOD

    void MovePassengers(bool beforeMovePlatform)
    {
        foreach (PassengerMovement passenger in passengerMovement)
        {
            // This ensures we only do ONE 'GetComponent' call for each passenger.
            if (!passengerDictionary.ContainsKey(passenger.transform))
            {
                passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Controller2D>());
            }

            if (passenger.moveBeforePlatform == beforeMovePlatform)
            {
                passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
            }
        }
    }

    void CalculatePassengerMovement(Vector3 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>(); // Each time we calc we want to see equal to new list of pass movements.

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        // Vertically moving platform
        if (velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                // If hit.distance is zero. The player is inside of this platform. Then don't run this collision code below... Do this for other two if(hits)
                if (hit && hit.distance != 0)
                {
                    if (!movedPassengers.Contains(hit.transform)) // HASHSET USED HERE.
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0;                    // ??? 10:30 E06 Video tutorial SL ######
                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

                        //hit.transform.Translate(new Vector3(pushX, pushY));                  // Actually moving the passenger. // will be swapped for controller move
                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
                    }
                }
            }
        }

        // Horizontally moving platform . Being pushed from side.
        if (velocity.x != 0)
        {
            // Based on VerticalCollisions
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                if (hit && hit.distance != 0) // Explanation in ... E10 @9:25
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = -skinWidth; // When this was 0 the passenger was never checking below unaware on ground. -skinWidth is small downward force.

                        //hit.transform.Translate(new Vector3(pushX, pushY));
                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
                        // horizontally moving so impossible false, movebefore true.
                    }
                }
            }
        }

        // Passenger on top of a horizontally or downward moving platform. Stop from falling "bouncing".
        if (directionY == -1 || velocity.y == 0 && velocity.x != 0)
        {
            // One skin width to get to the surface, and another for a small ray detecting anything standing on top. @19:00 E06 SL #
            float rayLength = skinWidth * 2;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                if (hit && hit.distance != 0) // Explanation in ... E10 @9:25
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        //hit.transform.Translate(new Vector3(pushX, pushY));
                        passengerMovement.Add(new PassengerMovement(hit.transform,new Vector3(pushX,pushY), true, false));
                                                                                                            //passenger moving down, move plat first.
                    }
                }
            }
        }
    } // END OF CalculatePassengerMovement METHOD

    struct PassengerMovement
    {
        public Transform transform;
        public Vector3 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatform;

        // Constructor! ...
        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            transform = _transform;
            velocity = _velocity;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
        }
    } // END OF PassengerMovement STRUCT

    void OnDrawGizmos()
    {
        if (localWaypoints != null)
        {
            Gizmos.color = gizmoColor;
            float size = .3f;

            for (int i = 0; i < localWaypoints.Length; i++)
            {
                Vector3 globalWaypointPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);                       // Draws a cross, horiz
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size); // vertical line
            }
        }
    }

}
