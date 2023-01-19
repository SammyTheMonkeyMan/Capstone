using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Transform target;

    public float lookAhead;
    public float dampen;
    private int direction;
    public float yOffset;

    public Transform farBackGround, middleBackGround;
    private Vector3 lastPos;

    //public float minHeight, maxHeight;

    //public bool stopFollow;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //if (!stopFollow)
        //{
        //transform.position = new Vector3(target.position.x, Mathf.Clamp(target.position.y, minHeight, maxHeight), transform.position.z);

        //transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        if (PlayerController.instance.theSR.flipX)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, target.position.x + lookAhead * direction, dampen + Mathf.Abs(PlayerController.instance.theRB.velocity.x) / 50), target.position.y + yOffset, transform.position.z);
        Vector3 amountToMove = transform.position - lastPos;

        farBackGround.position += amountToMove;
        middleBackGround.position += amountToMove * 0.5f;

        if (middleBackGround.position.x < transform.position.x - 11)
        {
            middleBackGround.position += Vector3.right * 11;
        }
        else if (middleBackGround.position.x > transform.position.x + 11)
        {
            middleBackGround.position += Vector3.right * -11;
        }

        lastPos = transform.position;
        //}
    }
}
