using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOffLedge : MonoBehaviour
{
    public float worldBottom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < worldBottom)
        {
            LevelManager.instance.RespawnPlayer();
        }
    }
}
