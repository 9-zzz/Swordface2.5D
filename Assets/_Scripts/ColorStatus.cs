using UnityEngine;
using System.Collections;

public class ColorStatus : MonoBehaviour
{

    float redValue = 1.0f;
    GameObject mesh;
    MeshRenderer mRend;
    Rigidbody2D rb2d;

    void Awake()
    {
        mesh = transform.GetChild(0).gameObject;
        mRend = mesh.GetComponent<MeshRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start()
    {
        mRend.materials[0].EnableKeyword("_EMISSION");
        mRend.materials[1].EnableKeyword("_EMISSION");

        //origColor = rend.materials[1].GetColor("_EmissionColor");
        mRend.materials[0].SetColor("_EmissionColor", new Color(redValue, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Bullet2D" && redValue > 0.0f)
        {
            redValue -= 0.1f;
            mRend.materials[0].SetColor("_EmissionColor", new Color(redValue, 0, 0));
            Destroy(coll.gameObject);
        }

        if (coll.gameObject.tag == "Bullet2D" && redValue <= 0.0f)
        {
            rb2d.AddForce(new Vector2(10, 100), ForceMode2D.Impulse);
            rb2d.AddTorque(500.0f, ForceMode2D.Impulse);
        }
    }

}
