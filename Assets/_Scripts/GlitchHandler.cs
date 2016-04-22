using UnityEngine;
using System.Collections;

public class GlitchHandler : MonoBehaviour
{
    public static GlitchHandler S;

    public float cd = 0.0f;
    public float cdSpeed = 8.0f;
    public float slj = 0.0f;
    public float sljSpeed = 8.0f;
    public float vj = 0.0f;
    public float vjSpeed = 8.0f;
    public float hs = 0.0f;
    public float hsSpeed = 0.1f;
    public float dgi = 0.0f;
    public float dgiSpeed = 0.1f;
    [Header("FX Settings")]
    public float cdWaitTime;
    public float cdMax;
    public float SLWaitTime;
    public float slMax;
    public bool rampUpCDGlitch = false;
    public bool rampUpSLGlitch = false;
    public bool dead = false;

    void Awake() { S = this; }

    void Start()
    {

    }

    public void ColorDriftFXMethod(float inMax)
    {
        cdMax = inMax;
        StartCoroutine(ColorDriftFX());
    }

    IEnumerator ColorDriftFX()
    {
        rampUpCDGlitch = true;
        yield return new WaitForSeconds(cdWaitTime);
        rampUpCDGlitch = false;
    }

    public void ScanLineFXMethod()
    {
        StartCoroutine(ScanLineFX());
    }

    IEnumerator ScanLineFX()
    {
        rampUpSLGlitch = true;
        yield return new WaitForSeconds(SLWaitTime);
        rampUpSLGlitch = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (rampUpCDGlitch) cd = Mathf.MoveTowards(cd, cdMax, Time.deltaTime * cdSpeed);
            if (!rampUpCDGlitch) cd = Mathf.MoveTowards(cd, 0.0f, Time.deltaTime * cdSpeed);

            if (rampUpSLGlitch) slj = Mathf.MoveTowards(slj, slMax, Time.deltaTime * sljSpeed);
            if (!rampUpSLGlitch) slj = Mathf.MoveTowards(slj, 0.0f, Time.deltaTime * sljSpeed);

            GetComponent<Kino.AnalogGlitch>().colorDrift = cd;
            GetComponent<Kino.AnalogGlitch>().scanLineJitter = slj;
            GetComponent<Kino.AnalogGlitch>().verticalJump = vj;
            GetComponent<Kino.AnalogGlitch>().horizontalShake = hs;
            //GetComponent<Kino.DigitalGlitch>().intensity = dgi;

            /*cd = Mathf.MoveTowards(cd, 0.25f, Time.deltaTime * cdSpeed);
            slj = Mathf.MoveTowards(slj, 0.35f, Time.deltaTime * sljSpeed);
            vj = Mathf.MoveTowards(vj, 0.01f, Time.deltaTime * vjSpeed);
            hs = Mathf.MoveTowards(hs, 0.01f, Time.deltaTime * hsSpeed);
            dgi = Mathf.MoveTowards(dgi, 0.02f, Time.deltaTime * dgiSpeed);*/
        }
    }

}
