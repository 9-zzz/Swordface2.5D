using UnityEngine;
using System.Collections;

public class FlyingEnemyTest : MonoBehaviour
{

    public float waitAndSpitTime;
    public float startMoveSpeed; // 7
    public float startRotateSpeed; // 100

    public int fHealth = 5;

    public GameObject fBullet;

    public bool activated = false;
    public bool hasReachedZ = false;
    public bool ready = false;

    Vector3 startTargetPosition;
    Transform sp;

    // Use this for initialization
    void Start()
    {
        sp = transform.GetChild(1).transform;

        startTargetPosition = transform.position;
        startTargetPosition.z = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!activated)
        {

            if (!hasReachedZ)
            {
                if (transform.position.z == 0)
                    hasReachedZ = true;

                transform.position = Vector3.MoveTowards(transform.position, startTargetPosition, Time.deltaTime * startMoveSpeed);
            }

            if (hasReachedZ) // and ready == false? // This just go 90 no further.
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), startRotateSpeed * Time.deltaTime);

                if (transform.rotation.y == 0)
                {
                    //hasReachedZ = false; // ??? To not all choroutine multiple times.
                    ready = true;
                    transform.GetChild(2).GetComponent<CircleCollider2D>().enabled = true;
                }
            }

            if (ready)
            {
                StartCoroutine(WaitAndSpit(waitAndSpitTime));
                ready = false;
                activated = true;
            }
        }
    }

    IEnumerator WaitAndSpit(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            Instantiate(fBullet, sp.position, sp.rotation);
            //var flyBullet = Instantiate(fBullet, sp.position, sp.rotation) as GameObject;
            //flyBullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(50, 20), ForceMode2D.Force);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Bullet2D")
        {
            if (fHealth <= 0)
                Destroy(gameObject);

            fHealth--;
        }
    }

}
