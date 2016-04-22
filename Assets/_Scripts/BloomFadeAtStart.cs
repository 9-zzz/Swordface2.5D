using UnityEngine;
using System.Collections;

public class BloomFadeAtStart : MonoBehaviour
{
    public static BloomFadeAtStart S;
    public float speed = 2.0f;
    public bool normal = false;
    float iThreshold;
    float iIntensity;
    float iRadius;

    void Awake()
    {
        S = this;
    }

    void Start()
    {
        //Destroy(this.GetComponent<BloomFadeAtStart>(), 5.0f);

        // Save initial values.
        iThreshold = GetComponent<Kino.Bloom>().thresholdGamma;
        iIntensity = GetComponent<Kino.Bloom>().intensity;
        iRadius = GetComponent<Kino.Bloom>().radius;

        BloomFade();
    }

    public void BloomFade()
    {
        // Set crazy values.
        GetComponent<Kino.Bloom>().thresholdGamma = 0.0f;
        GetComponent<Kino.Bloom>().intensity = 42.0f;
        GetComponent<Kino.Bloom>().radius = 7.0f;
        normal = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!normal)
        {
            GetComponent<Kino.Bloom>().thresholdGamma = Mathf.MoveTowards(GetComponent<Kino.Bloom>().thresholdGamma, iThreshold, Time.deltaTime * speed);
            GetComponent<Kino.Bloom>().intensity = Mathf.MoveTowards(GetComponent<Kino.Bloom>().intensity, iIntensity, Time.deltaTime * (speed * 15));
            GetComponent<Kino.Bloom>().radius = Mathf.MoveTowards(GetComponent<Kino.Bloom>().radius, iRadius, Time.deltaTime * speed);
        }

        if (GetComponent<Kino.Bloom>().radius == 1.5f)
            normal = true;

   }

}
