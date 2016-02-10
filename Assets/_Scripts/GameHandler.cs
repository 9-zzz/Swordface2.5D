using UnityEngine;
using System.Collections;

public class GameHandler : MonoBehaviour
{
    public static GameHandler S;

    public float fallDistance = -14.0f;

    GameObject playerRef;

    void Awake()
    {
        S = this;
    }

    // Use this for initialization
    void Start()
    {
        playerRef = Player.S.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Fader.S.RestartFadeMethod();

        if (playerRef.transform.position.y <= fallDistance)
            Fader.S.RestartFadeMethod();
    }

}
