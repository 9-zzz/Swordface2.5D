using UnityEngine;
using System.Collections;

public class FlyingEnemyDeathCollider : MonoBehaviour
{
    //Rigidbody2D playerRB;
    public GameObject FlyingEnemyFractured;
    bool hitFlag = false;

    void Awake()
    {
        //playerRB = Player.S.GetComponent<Rigidbody>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !hitFlag)
        {
            Player.S.ExternalJump(60.0f);
            var fef = Instantiate(FlyingEnemyFractured, transform.position, transform.rotation) as GameObject;
            Destroy(fef, 4.0f);
            //transform.parent = null;
            hitFlag = true;
            Destroy(transform.parent.gameObject); // Destroys self too.
        }
    }

}
