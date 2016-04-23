using UnityEngine;
using System.Collections;

public class FallCrasher : MonoBehaviour
{
    Rigidbody2D rb2d;
    public float force;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddForce(-transform.up*force, ForceMode2D.Impulse);
    }

    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        GlitchHandler.S.ColorDriftFXMethod(1);
        rb2d.isKinematic = true;
    }

}
