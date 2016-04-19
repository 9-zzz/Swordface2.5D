using UnityEngine;
using System.Collections;

public class MoonMustacheMonster : MonoBehaviour
{
    public GameObject Player;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Player.transform.position);
    }

}
