using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake S;
  // Transform of the camera to shake. Grabs the gameObject's transform
  // if null.
  public Transform camTransform;

  // How long the object should shake for.
  public float shake = 0f;

  // Amplitude of the shake. A larger value shakes the camera harder.
  public float shakeAmount = 0.7f;
  public float decreaseFactor = 1.0f;

  Vector3 originalPos;

    void Awake()
    {
        S = this;

        originalPos = this.transform.position;

        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    //void OnEnable() { originalPos = camTransform.localPosition; }

    /*
     *   Am I changing the Iso camera position when I disable these things?
     *   Or does the constant updating in the other script for following
     *   the player fix that?
     *   Otherwise everytime i shake the camera should be off set... 
     */

    void Update()
    {
        if (shake > 0)
        {
            //camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            camTransform.localPosition += Random.insideUnitSphere * shakeAmount;            // So I can still have the camera follow the player.
            shake -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shake = 0f;
            //camTransform.localPosition = originalPos;						// Again, to enable camera player following.
            transform.position = Vector3.MoveTowards(transform.position, originalPos, Time.deltaTime * 5.0f);
        }
    }

}
