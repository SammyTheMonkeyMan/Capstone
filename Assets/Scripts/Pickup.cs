using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool isGem, isHeal;

    private bool isCollected = false;

    public GameObject pickupEffect;

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
        if (other.CompareTag("Player") && !isCollected)
        {
            if (isGem)
            {
                LevelManager.instance.gemsCollected++;
                UIController.instance.UpdateGemDisplay();
                isCollected = true;
                Destroy(gameObject);
                Instantiate(pickupEffect, transform.position, transform.rotation);
                AudioManager.instance.PlaySFX("Pickup Gem");
            }
            /*else if (isHeal && PlayerHealthController.instance.currentHealth < PlayerHealthController.instance.maxHealth)
            {
                PlayerHealthController.instance.HealPlayer();
                UIController.instance.UpdateHealthDisplay();
                isCollected = true;
                Destroy(gameObject);
                Instantiate(pickupEffect, transform.position, transform.rotation);
                AudioManager.instance.PlaySFX("Pickup Health");
            }*/
        }
    }
}
