using UnityEngine;
using System.Collections;

public class GiraBrain : MonoBehaviour
{
    Animator ani;
    Renderer ren;
    public float distaneFromPlayer;
    public GameObject sp;
    public GameObject spBlast;
    public GameObject headBone;
    ParticleSystem spPS;
    ParticleSystem spPSBlast;
    public GameObject GiraBullet;
    public Color lerpEmissionColor;
    public Color lerpEmissionColorTarget;
    public bool shooting = false;
    int ammo = 3;

    // Use this for initialization
    void Start()
    {
        ani = GetComponent<Animator>();
        ren = transform.GetChild(1).GetComponent<Renderer>();
        StartCoroutine(waitAndShoot());
        spPS = sp.GetComponent<ParticleSystem>();
        spPSBlast = spBlast.GetComponent<ParticleSystem>();
        //ani.enabled = false;
    }

    IEnumerator waitAndShoot()
    {
        yield return new WaitForSeconds(2.0f);
        ani.SetBool("isFiringBullets", true);
        while (true)
        {
            if(ammo == 0)
                ani.SetBool("isFiringBullets", false);
            if (shooting && ammo > 0)
            {
                spPS.Play();
                yield return new WaitForSeconds(2.7f);
                spPSBlast.Play();

                var gb = Instantiate(GiraBullet, sp.transform.position, GiraBullet.transform.rotation)as GameObject;
                gb.transform.parent = transform.GetChild(0);

                ammo--;
                yield return new WaitForSeconds(0.75f);
            }
            yield return new WaitForSeconds(0.01f);
        }
        //yield return new WaitForSeconds(0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        distaneFromPlayer = Vector3.Distance(transform.position, Player.S.transform.position);

        if (distaneFromPlayer < 8)
        {
            shooting = false;
            ani.SetBool("isAttacking", true);
            lerpEmissionColorTarget = Color.red;
        }
        else if (distaneFromPlayer > 10)
        {
            lerpEmissionColorTarget = Color.red;
            sp.transform.LookAt(Player.S.transform.position);
            //headBone.transform.LookAt(Player.S.transform.position);
            shooting = true;
        }
        else
        {
            shooting = false;
            ani.SetBool("isAttacking", false);
            lerpEmissionColorTarget = Color.black;
        }


        ren.materials[0].SetColor("_EmissionColor", lerpEmissionColor);
        ren.materials[3].SetColor("_EmissionColor", lerpEmissionColor);

        lerpEmissionColor = Color.Lerp(lerpEmissionColor, lerpEmissionColorTarget, Time.deltaTime * 2.0f);
    }

}
