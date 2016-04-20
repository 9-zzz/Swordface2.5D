using UnityEngine;
using System.Collections;

public class CrackFloor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "lpsword1" && LPSwordAnimTest.S.anim.GetBool("slashDown"))
        {
            GetComponent<Collider2D>().enabled = false;


        }
    }


}
