using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    // Makes light intensity and range flicker with perlin noise and time.
    public Light lightToFlicker;
    public float random;
    public float minIntensity;
    public float maxIntensity;
    public float minRange;
    public float maxRange;

    void Awake()
    {
        lightToFlicker = this.GetComponent<Light>();
    }

    void Start()
    {
        random = Random.Range(0.0f, 65535.0f); // Some type of random seed for perlin noise?
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(random, Time.time); // I don't understand this...
        lightToFlicker.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
        lightToFlicker.range = Mathf.Lerp(minRange, maxRange, noise);
    }

}
