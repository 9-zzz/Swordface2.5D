using UnityEngine;
using System.Collections;

public class Bullet2D : MonoBehaviour
{
    public float xForce = 5.0f;
    public float lifeTime = 2.0f;
    Rigidbody2D rb2D;
    Vector2 bulletVector;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (Controller2D.S.facing == -1)
            xForce *= -1.0f;

        if (Controller2D.S.facing == 1)
            xForce = Mathf.Abs(xForce);

        Destroy(gameObject, lifeTime);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            bulletVector = new Vector2(0, Mathf.Abs(xForce));
        }
        else
        {
            bulletVector = new Vector2(xForce, 0);
        }

        //rb2D.AddRelativeForce(bulletVector, ForceMode2D.Impulse);
        rb2D.AddForce(bulletVector, ForceMode2D.Impulse);
    }

    void Update()
    {

    }

}
