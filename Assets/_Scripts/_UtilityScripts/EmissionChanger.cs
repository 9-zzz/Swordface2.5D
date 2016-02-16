using UnityEngine;
using System.Collections;

public class EmissionChanger : MonoBehaviour
{
    public Color newEmCol;
    public Color origColor;

    public int materialIndex;

    Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void Start()
    {
        this.GetComponent<Renderer>().materials[1].EnableKeyword("_EMISSION");

        origColor = rend.materials[1].GetColor("_EmissionColor");

        StartCoroutine(Flicker());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator Flicker()
    {
        for (int i = 0; i < 30; i++)
        {
            rend.materials[1].SetColor("_EmissionColor", newEmCol);
            yield return new WaitForSeconds(0.02f);
            rend.materials[1].SetColor("_EmissionColor", origColor);
            yield return new WaitForSeconds(0.02f);
        }
    }

}
