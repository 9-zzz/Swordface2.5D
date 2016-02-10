using UnityEngine;
using System.Collections;

public class LPManFaceBeam : MonoBehaviour
{

    public float zScale;
    public float shootSpeed;
    public bool firing = false;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(faceFire());
    }

    // Update is called once per frame
    void Update()
    {

        transform.localScale = new Vector3(1, 1, zScale);

        //if(Input.GetKey(KeyCode.K))
        if (firing)
        {
            zScale = Mathf.MoveTowards(zScale, 20, Time.deltaTime * shootSpeed);
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
        else
        {
            zScale = 0;
            transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        }

    }

    IEnumerator faceFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            firing = true;
            yield return new WaitForSeconds(7.0f);
            firing = false;
        }
    }

}
