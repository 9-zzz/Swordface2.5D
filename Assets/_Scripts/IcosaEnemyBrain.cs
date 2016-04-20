using UnityEngine;
using System.Collections;

public class IcosaEnemyBrain : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb2d;
    AudioSource aud;
    public float waitTime;
    public bool hit = false;
    ParticleSystem jps;

    public Vector2 dirVector= new Vector2(-0.1f, 0.75f);

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
        jps = transform.GetChild(2).GetComponent<ParticleSystem>();
    }

    void Start()
    {
        StartCoroutine(AngryCycle());
    }

    void Update()
    {
        if (hit)
        {
            //var rotspeed = 0.0f;
            //rotspeed += 9.0f;
            //transform.Rotate(transform.up * rotspeed);
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0, 25, 0), Time.deltaTime * 60.0f);
            transform.position += Random.insideUnitSphere * 0.2f;
            if(transform.localScale.x == 0)
                Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "lpsword1")
        {
            aud.Play();
            jps.gameObject.SetActive(false);
            hit = true;
            rb2d.isKinematic = true;
        }
    }

    IEnumerator AngryCycle()
    {
        while (true && !hit)
        {
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("isAngry", false);
            yield return new WaitForSeconds(waitTime);
            anim.SetBool("isAngry", true);
            yield return new WaitForSeconds(0.5f);
            rb2d.AddForce(dirVector * 50.0f, ForceMode2D.Impulse);
            jps.Play();
        }
    }

}
