using UnityEngine;
using System.Collections;

public class FlyingFishBrain : MonoBehaviour
{
    Vector3 wp;

    void Start()
    {
        wp = transform.position + transform.forward * 500;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, wp, Time.deltaTime * 20.0f);
    }

}
