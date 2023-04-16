using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public GameObject deathEffect;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //PlayerHealthController.instance.DealDamage();
            //LevelManager.instance.RespawnPlayer();
            PlayerController.instance.anim.SetTrigger("getHurt");
            AudioManager.instance.PlaySFX("Player Death");
            Instantiate(deathEffect, PlayerController.instance.transform.position, deathEffect.transform.rotation);
            StartCoroutine(KillPlayer());
        }
    }

    private IEnumerator KillPlayer()
    {
        yield return new WaitForSeconds(0.3f);
        LevelManager.instance.RespawnPlayer();
    }
}
