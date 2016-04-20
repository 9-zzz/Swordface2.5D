using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerCollider2D : MonoBehaviour
{
    public static PlayerCollider2D S;

    public Text nvm;

    public int health = 3;
    public float hurtFlashTime;
    MeshRenderer playerMeshRenderer;
    public float duration;
    public Color emColor;
    public Color regColor;

    void Awake()
    {
        S = this;
        playerMeshRenderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        nvm.CrossFadeAlpha(0, 0, true);
    }

    void Update()
    {
        if (health < 3)
        {
            float lerp = (Mathf.PingPong(Time.time, duration) / duration);
            lerp /= (health * 0.5f);
            emColor = Color.Lerp(Color.black, Color.red, lerp);
            regColor = Color.Lerp(Color.white, Color.red, lerp);

            playerMeshRenderer.material.SetColor("_EmissionColor", emColor);
            playerMeshRenderer.material.color = regColor;
        }

    }

    public void ResetPlayerColor()
    {
        emColor = Color.black;
        regColor = Color.white;
        playerMeshRenderer.material.SetColor("_EmissionColor", emColor);
        playerMeshRenderer.material.color = regColor;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "FlyingEnemyBullet" || coll.gameObject.tag == "IcosaEnemy")
            StartCoroutine(PlayerHurt(9));
    }

    IEnumerator PlayerHurt(int flashes)
    {
        health--;
        if (health == 0)
        {
            nvm.CrossFadeAlpha(0.6f, 0.7f, true);
            GlitchHandler.S.dead = true;
            GetComponent<Controller2D>().enabled = false;
            GetComponent<Player>().enabled = false;
            Camera.main.GetComponent<Kino.AnalogGlitch>().colorDrift = 0.5f;
            Camera.main.GetComponent<Kino.AnalogGlitch>().verticalJump = 0.15f;
            Fader.S.restartFadeTime = 0.75f;
            Fader.S.RestartFadeMethod();
        }

        for (int i = 0; i < flashes; i++)
        {
            playerMeshRenderer.enabled = false;
            yield return new WaitForSeconds(hurtFlashTime);
            playerMeshRenderer.enabled = true;
            yield return new WaitForSeconds(hurtFlashTime);
        }
    }

}
