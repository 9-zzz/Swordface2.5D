using UnityEngine;
using System.Collections;

public class LPManFaceBeam : MonoBehaviour {

    public float zScale;
    public float shootSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

            transform.localScale = new Vector3(1, 1, zScale);

        if(Input.GetKey(KeyCode.K))
        {
            zScale = Mathf.MoveTowards(zScale, 10, Time.deltaTime * shootSpeed);
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
        else
        {
            zScale = 0;
            transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        }
	
	}
}
