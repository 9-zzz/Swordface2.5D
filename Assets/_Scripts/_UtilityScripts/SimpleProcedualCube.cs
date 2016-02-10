using UnityEngine;
using System.Collections;

public class SimpleProcedualCube : MonoBehaviour
{

    public GameObject pc;
    public float dist;
    public GameObject player;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            dist = Vector3.Distance(transform.position, player.transform.position);

            if (dist > 20)
                Destroy(transform.parent.gameObject);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            Instantiate(pc, transform.position + (transform.forward * 11.0f), transform.rotation);
            //Tobias sucks
            GetComponent<Collider>().enabled = false;
        }

    }
}
