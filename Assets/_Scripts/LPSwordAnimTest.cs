using UnityEngine;
using System.Collections;

public class LPSwordAnimTest : MonoBehaviour
{

    Animator anim;
    public float waitTime;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {

    }

    IEnumerator SetStateAndWait(string state, int s)
    {
        transform.GetChild(0).GetComponent<TrailRenderer>().enabled = true;
        anim.SetInteger(state, s);
        yield return new WaitForSeconds(waitTime);
        anim.SetInteger(state, 0);
        yield return new WaitForSeconds(0.2f);
        transform.GetChild(0).GetComponent<TrailRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            StartCoroutine(SetStateAndWait("s1", 1));

        if (Input.GetKeyDown(KeyCode.C))
            StartCoroutine(SetStateAndWait("s2", 2));
    }

}
