using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Fader : MonoBehaviour
{
    public static Fader S;

    public Color imageColor;
    public float startingFadeOutTime = 3.0f;
    public float restartFadeTime = 3.0f;
    public float fadeToBlackTime = 3.0f;
    public float flashFadeTime = 0.25f;

    Image thisImage;

    void Awake()
    {
        S = this;
        thisImage = GetComponent<Image>();
        thisImage.color = imageColor;
    }

    void Start() { thisImage.CrossFadeAlpha(0, startingFadeOutTime, true); }

    public void FlashFadeMethod() { StartCoroutine(flashFade()); }
    IEnumerator flashFade()
    {
        thisImage.CrossFadeAlpha(1, flashFadeTime, true);
        yield return new WaitForSeconds(flashFadeTime);
        thisImage.CrossFadeAlpha(0, fadeToBlackTime, true);
    }


    public void RestartFadeMethod() { StartCoroutine(restartFade()); }
    IEnumerator restartFade()
    {
        thisImage.CrossFadeAlpha(1, restartFadeTime, true);
        yield return new WaitForSeconds(restartFadeTime + 0.25f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void FadeToBlackMethod() { StartCoroutine(fadeToBlack()); }
    IEnumerator fadeToBlack()
    {
        thisImage.CrossFadeAlpha(1, fadeToBlackTime, true);
        yield return new WaitForSeconds(fadeToBlackTime);
    }

}
