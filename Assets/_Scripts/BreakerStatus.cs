using UnityEngine;
using System.Collections;

public class BreakerStatus : MonoBehaviour
{

    int childctr = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Bullet2D" && (childctr < 4))
        {
            Destroy(coll.gameObject);
            transform.GetChild(childctr).GetComponent<PushFracturePieces>().enabled = true;
            //transform.GetChild(childctr).transform.parent = null;
            childctr++;

            if (childctr == 3)
                Destroy(gameObject, 2.0f);
        }
    }

}
