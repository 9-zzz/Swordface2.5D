using UnityEngine;
using System.Collections;

public class LPSwordAnimTest : MonoBehaviour
{
    public static LPSwordAnimTest S;

    public Animator anim;
    public float waitTime;
    TrailRenderer swordTR;
    Collider2D swordCol;

    void Awake()
    {
        S = this;
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        swordTR = transform.GetChild(0).GetComponent<TrailRenderer>();
        swordCol = transform.GetChild(1).GetComponent<Collider2D>();
    }

    IEnumerator SetStateAndWait(string state, int s)
    {
        swordTR.enabled = true;
        swordCol.enabled = true;
        anim.SetInteger(state, s);
        yield return new WaitForSeconds(waitTime);
        anim.SetInteger(state, 0);
        yield return new WaitForSeconds(0.2f);
        swordTR.enabled = false;
        swordCol.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && !(Input.GetKey(KeyCode.DownArrow)))
            StartCoroutine(SetStateAndWait("s2", 2));

        // Downward sword attack code
        if (Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.DownArrow))
        {
            if (Player.S.isDownSlamming == false)
            {
                Player.S.isDownSlamming = true;
                swordTR.enabled = true;
                swordCol.enabled = true;
                Player.S.DownSlamMethod(-100.0f);
                anim.SetBool("slashDown", true);
            }
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            swordTR.enabled = false;
            swordCol.enabled = false;
            anim.SetBool("slashDown", false);
        }
    }

}
