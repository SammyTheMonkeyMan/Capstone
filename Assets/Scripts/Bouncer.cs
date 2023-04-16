using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    private Animator anim;
    public float bounce;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // PlayerController.instance.theRB.velocity = new Vector2(PlayerController.instance.theRB.velocity.x, bounce);
            PlayerController.instance.theRB.velocity = new Vector2(PlayerController.instance.theRB.velocity.x, 0);
            PlayerController.instance.theRB.AddForce(transform.up * bounce + Vector3.up * bounce / 5, ForceMode2D.Impulse);
            anim.SetTrigger("Bounce");
            AudioManager.instance.PlaySFX("Player Jump");
        }
    }
}
