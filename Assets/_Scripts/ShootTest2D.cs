using UnityEngine;
using System.Collections;

public class ShootTest2D : MonoBehaviour
{
    public GameObject sp2D;
    public GameObject bullet2D;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            Instantiate(bullet2D, sp2D.transform.position, sp2D.transform.rotation);
    }

}
