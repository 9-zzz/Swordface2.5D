using UnityEngine;
using System.Collections;

public class GiraBullet : MonoBehaviour
{
    public float force;
    Rigidbody2D rb2d;
    public Vector3 target;
    Vector3 startingPosition;
    bool hit = false;

    void Start()
    {
        startingPosition = transform.position;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddRelativeForce((Vector2.left * force), ForceMode2D.Impulse);
        //Destroy(gameObject, 15);
    }

    void Update()
    {
        transform.LookAt(target);
        rb2d.AddRelativeForce(transform.forward*6);

        if (!hit)
            target = Player.S.transform.position;

        if (hit)
        {
            target = startingPosition;
            if (Vector2.Distance(transform.position, target) > 5)
                rb2d.velocity /= 1.01f;

        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "lpsword1")
        {
            GetComponent<Collider2D>().enabled = false;
            rb2d.AddForce(transform.forward * (force * -3), ForceMode2D.Impulse);
            hit = true;
            //Destroy(gameObject, 2.0f);
        }
    }

}
