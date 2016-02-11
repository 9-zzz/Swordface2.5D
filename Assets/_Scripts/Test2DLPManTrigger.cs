using UnityEngine;
using System.Collections;

public class Test2DLPManTrigger : MonoBehaviour
{

    public GameObject lpManRef;
    Renderer lpRend;
    Color[] lpColors;

    void Start()
    {
        lpRend = lpManRef.GetComponent<Renderer>();
        lpColors = new Color[4];

        for(int i = 0;  i < lpRend.materials.Length; i++)
        {
            lpColors[i] = lpRend.materials[i].color;
        }
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet2D")
        {
            StartCoroutine(redFlashMaterials());
        }
    }

    IEnumerator redFlashMaterials()
    {
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < lpRend.materials.Length; i++)
            {
                lpRend.materials[i].color = Color.red;
                yield return new WaitForSeconds(0.02f);
                lpRend.materials[i].color = lpColors[i];
                yield return new WaitForSeconds(0.02f);
            }
        }
    }

}
