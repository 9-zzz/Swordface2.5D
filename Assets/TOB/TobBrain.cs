using UnityEngine;
using System.Collections;

public class TobBrain : MonoBehaviour
{

    Animator anim;

    void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            anim.SetBool("run", true);

        if (anim.GetBool("run"))
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * 100);
    }

}
