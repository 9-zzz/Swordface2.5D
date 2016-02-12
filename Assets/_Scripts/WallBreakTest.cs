using UnityEngine;
using System.Collections;

public class WallBreakTest : MonoBehaviour {

    GameObject masterParent;
    public Rigidbody[] rbs;

    // Use this for initialization
    void Start()
    {
        masterParent = transform.GetChild(0).gameObject;
        rbs = masterParent.GetComponentsInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetOff()
    {
      foreach (Rigidbody rb in rbs)
        {
            rb.useGravity = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet2D")
        {
            print("poo");
            SetOff();
        }
    }
}
