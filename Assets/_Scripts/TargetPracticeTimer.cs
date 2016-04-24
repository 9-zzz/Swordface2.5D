using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TargetPracticeTimer : MonoBehaviour
{
    public static TargetPracticeTimer S;
    Text t;
    static TimeSpan timeSpan;
    public GameObject[] healthPickups;
    public int targetCtr;
    bool f = false;
    ParticleSystem NextLevelFire;

    void Awake()
    {
        S = this;
        healthPickups= GameObject.FindGameObjectsWithTag("healthPickup");
        targetCtr = healthPickups.Length;
    }

    void Start()
    {
        NextLevelFire = GameObject.Find("NextLevelFire").GetComponent<ParticleSystem>();
        t = GetComponent<Text>();
   }

    IEnumerator flashTimeText()
    {
        while (true)
        {
            t.enabled = false;
            yield return new WaitForSeconds(0.05f);
            t.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
    }

    void LateUpdate()
    {
        if (targetCtr != 0)
        {
            t.text = SetTimeSpan(Time.timeSinceLevelLoad);
        }
        else if(!f)
        {
            StartCoroutine(flashTimeText());
            NextLevelFire.Play();
            f = !f;
        }
    }

    public static string SetTimeSpan(float time)
    {
        // Create TimeSpan variable
        timeSpan = System.TimeSpan.FromSeconds(time);
        return string.Format("{0:D2}:{1:D2}:{2:00}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);

    }

}
