using UnityEngine;
using System.Collections;

// This prefab is on a LAYER that ignores its own LAYER and FlyingEnemy's LAYER.
// So bullets can't hit each other and can't hit the bullet shooter.
public class FlyingEnemyBullet : MonoBehaviour
{
    public float lifeTime = 2.0f;
    public Vector2 bulletVector;
    public GameObject FlyingBulletDeathPS;

    Rigidbody2D rb2D;
    ParticleSystem childPS;

    void Awake()
    {
        //playerRef = Player.S;
        rb2D = GetComponent<Rigidbody2D>();
        childPS = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    void Start()
    {
        rb2D.AddForce(bulletVector, ForceMode2D.Impulse);
    }

    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        childPS.Stop();
        childPS.gameObject.transform.parent = null;
        Instantiate(FlyingBulletDeathPS, transform.position, Quaternion.Euler(-90, 0, 0)); // This is iffy... :\
        Destroy(gameObject);
    }

}
