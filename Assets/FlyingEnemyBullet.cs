using UnityEngine;
using System.Collections;

public class FlyingEnemyBullet : MonoBehaviour
{
    public float lifeTime = 2.0f;
    Rigidbody2D rb2D;
    public Vector2 bulletVector;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        //playerRef = Player.S;
    }

    // Use this for initialization
    void Start()
    {
        rb2D.AddForce(bulletVector, ForceMode2D.Impulse);

    }

    // Update is called once per frame
    void Update()
    {

    }

}
