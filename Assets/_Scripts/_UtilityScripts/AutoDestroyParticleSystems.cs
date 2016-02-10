using UnityEngine;
using System.Collections;

public class AutoDestroyParticleSystems : MonoBehaviour
{
    void Update()
    {
        if (!this.GetComponent<ParticleSystem>().IsAlive())
            Destroy(gameObject);
    }
}
