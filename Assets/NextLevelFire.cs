using UnityEngine;
using System.Collections;

public class NextLevelFire : MonoBehaviour
{
    public int index;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && TargetPracticeTimer.S.targetCtr == 0)
            Fader.S.toLevelFadeMethod(index);
    }

}
