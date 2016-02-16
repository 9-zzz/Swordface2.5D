using UnityEngine;
using System.Collections;

public class PlayerCollision2D : MonoBehaviour
{

    public float hurtFlashTime;

    MeshRenderer playerMeshRenderer;

    void Awake()
    {
        playerMeshRenderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "FlyingEnemyBullet")
            StartCoroutine(PlayerHurt(5));
    }

    IEnumerator PlayerHurt(int flashes)
    {
        for (int i = 0; i < flashes; i++)
        {
            playerMeshRenderer.enabled = false;
            yield return new WaitForSeconds(hurtFlashTime);
            playerMeshRenderer.enabled = true;
            yield return new WaitForSeconds(hurtFlashTime);
        }
    }

}
