using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour
{
    // All purpose simple auto rotator for all axes. I should look into Slerp.
    public Vector3 rotationVector;

    void Update()
    {
        transform.Rotate((rotationVector * Time.deltaTime));
    }

}
