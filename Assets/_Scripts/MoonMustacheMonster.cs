using UnityEngine;
using System.Collections;

public class MoonMustacheMonster : MonoBehaviour
{
    public GameObject Player;
    public Color[] colors;
    Renderer rend;
    public float lerp, duration;
    public int i = 0;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void Start()
    {
        StartCoroutine(Color());
    }

    void Update()
    {
        transform.LookAt(Player.transform.position);

        //rend.materials[Random.Range(0, 9)].SetColor("_EmissionColor", colors[Random.Range(0, 9)]);
        lerp = (Mathf.PingPong(Time.time, duration) / duration);
    }

    IEnumerator Color()
    {
        while (true)
        {
            //rend.materials[Random.Range(0, 9)].SetColor("_EmissionColor", colors[Random.Range(0, 9)]);
            rend.materials[Random.Range(0, 8)].SetColor("_EmissionColor", colors[i]);
            yield return new WaitForSeconds(0.001f);
            i++;
            if (i == 8)
                i = 0;
        }
    }

}
