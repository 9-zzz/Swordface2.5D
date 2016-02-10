using UnityEngine;
using System.Collections;

public class EmissionChanger : MonoBehaviour
{
    public Color oHeartColor;
    public Color oOutColor;
    public Color emCol;
    public Color emCol2;
    public Color newEmCol;
    Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    // Use this for initialization
    void Start()
    {
        this.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    }

    // Update is called once per frame
    void Update()
    {
        //DynamicGI.SetEmissive(rend.materials[3].color, emCol*1.0f);
        rend.materials[3].SetColor("_EmissionColor", emCol);
        rend.materials[4].SetColor("_EmissionColor", emCol2);

        if (Input.GetKey(KeyCode.C))
        {
            emCol = Color.Lerp(emCol, newEmCol, (1.7f * Time.deltaTime));
            //P_Camera.Instance.shake = 2;
        }
        else
        {
            emCol = Color.Lerp(emCol, Color.black, (1.7f * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.X))
            emCol2 = Color.Lerp(emCol2, newEmCol, (1.7f * Time.deltaTime));
        else
            emCol2 = Color.Lerp(emCol2, Color.black, (1.7f * Time.deltaTime));





    }


}
