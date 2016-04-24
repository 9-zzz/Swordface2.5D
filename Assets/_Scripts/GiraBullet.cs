using UnityEngine;
using System.Collections;

public class GiraBullet : MonoBehaviour
{
    public float force;
    Rigidbody2D rb2d;
    public Vector2 target;
    //Vector2 startingPosition;
    bool hit = false;

    void Start()
    {
        //startingPosition = transform.position;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddRelativeForce((Vector2.left * force), ForceMode2D.Impulse);
        //Destroy(gameObject, 15);
    }

    void Update()
    {
        transform.LookAt(target);
        rb2d.AddRelativeForce(transform.forward * force);

        if (!hit)
            target = Player.S.transform.position;

        if (hit)
        {
            //target = startingPosition + new Vector2(-0.1f,-0.5f);
            target = transform.parent.position;
            if (Vector2.Distance(transform.position, target) < 7)
                transform.position = Vector2.Lerp(transform.position, target, Time.deltaTime * 5);
                //force = 1/(Vector2.Distance(transform.position, target));

            //transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * 10);

            if (new Vector2(transform.position.x,transform.position.y) == target)
                Destroy(gameObject, 1);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "lpsword1")
        {
            GetComponent<Collider2D>().enabled = false;
            rb2d.AddForce(Vector2.up * (force * 1), ForceMode2D.Impulse);
            hit = true;
            //Destroy(gameObject, 2.0f);
        }
    }

}
