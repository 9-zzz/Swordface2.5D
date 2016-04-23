using UnityEngine;
using System.Collections;

public class HealthPickUp_1 : MonoBehaviour
{
    int playerHealthMax = 3;
    bool gotted = false;
    public float scaleSpeed = 70.0f;
    AudioSource aud;

    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "lpsword1")
        {
            TargetPracticeTimer.S.targetCtr--;
            aud.pitch = Random.Range(0.9f, 1.1f);
            aud.Play();
            if (PlayerCollider2D.S.health < playerHealthMax)
            {
                PlayerCollider2D.S.health++;
                PlayerCollider2D.S.duration *= 3;

                if (PlayerCollider2D.S.health == playerHealthMax)
                    PlayerCollider2D.S.ResetPlayerColor();
            }
            gotted = true;
            GetComponent<AutoRotate>().rotationVector.z *= 10;
            BloomFadeAtStart.S.speed = 20;
            BloomFadeAtStart.S.BloomFade();
            GetComponent<Collider2D>().enabled = false;
        }
    }

    void Update()
    {
        if (gotted)
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0, 0, 100), Time.deltaTime * scaleSpeed);

        if (transform.localScale.x == 0)
            Destroy(gameObject);
    }

}
