using UnityEngine;
using System.Collections;

public class HealthPickUp_1 : MonoBehaviour
{
    int playerHealthMax = 3;

    void Start()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerCollider2D.S.health++;
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if (PlayerCollider2D.S.health < playerHealthMax)
            {
                PlayerCollider2D.S.health++;
                if (PlayerCollider2D.S.health == playerHealthMax)
                    PlayerCollider2D.S.ResetPlayerColor();
            }
            Destroy(gameObject);
        }
    }

    void Update()
    {

    }

}
