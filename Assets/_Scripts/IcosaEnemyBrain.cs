using UnityEngine;
using System.Collections;

public class IcosaEnemyBrain : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb2d;
    public float waitTime;
    ParticleSystem jps;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jps = transform.GetChild(2).GetComponent<ParticleSystem>();
    }

    void Start()
    {
        StartCoroutine(AngryCycle());
    }

    void Update()
    {

    }

    IEnumerator AngryCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("isAngry", false);
            yield return new WaitForSeconds(waitTime);
            anim.SetBool("isAngry", true);
            yield return new WaitForSeconds(0.5f);
            rb2d.AddForce(Vector2.up*50.0f,ForceMode2D.Impulse);
            jps.Play();
        }
    }

}
