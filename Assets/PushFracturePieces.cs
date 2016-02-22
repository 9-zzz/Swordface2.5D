using UnityEngine;
using System.Collections;

public class PushFracturePieces : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Rigidbody>().AddForce( 8.0f * (transform.GetChild(i).transform.position - transform.position), ForceMode.Impulse);
            Destroy(transform.GetChild(i).gameObject, 4.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
