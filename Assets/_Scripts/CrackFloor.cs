using UnityEngine;
using System.Collections;

public class CrackFloor : MonoBehaviour
{
    public Component[] rbs;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "lpsword1" && LPSwordAnimTest.S.anim.GetBool("slashDown"))
        {
            GetComponent<Collider2D>().enabled = false;
            rbs = transform.GetChild(0).GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rbs)
            {
                rb.isKinematic = false;
                Destroy(rb.gameObject, 5.0f);
            }

        }
    }


}
