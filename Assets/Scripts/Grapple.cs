using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    public Sprite pullGrappleOn, pullGrappleOff, swingGrappleOn, swingGrappleOff;

    private DistanceJoint2D grapple;
    //public GameObject grapplePoints;
    private GameObject[] swingGrapplePoints, pullGrapplePoints;
    private GameObject currentGrapplePoint;
    public float grapplePullForce;
    private bool isPullGrapple = true;
    public float grapplePop;
    public float minGrappleDistance, maxGrappleDistance;
    //private Color pullGrappleColour, swingGrappleColour;
    //private bool canGrapplePop;
    private Vector2 previousDirection;
    public float gravityReduction;
    private float regularGravity;

    private LineRenderer rope;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    private float ropeSegLen = 0.25f;
    private int segmentLength = 35;
    private float lineWidth = 0.1f;

    PlayerInputActions playerInputActions;

    // Start is called before the first frame update
    void Start()
    {
        swingGrapplePoints = GameObject.FindGameObjectsWithTag("Swing Grapple Point");
        pullGrapplePoints = GameObject.FindGameObjectsWithTag("Pull Grapple Point");
        grapple = GetComponent<DistanceJoint2D>();
        currentGrapplePoint = pullGrapplePoints[0];
        rope = GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        for (int i = 0; i < segmentLength; i++)
        {
            ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }
        //pullGrappleColour = pullGrapplePoints[0].GetComponent<SpriteRenderer>().color;
        //swingGrappleColour = swingGrapplePoints[0].GetComponent<SpriteRenderer>().color;
        playerInputActions = new PlayerInputActions();
        playerInputActions.ActionMap.Enable();
        regularGravity = PlayerController.instance.theRB.gravityScale;
    }

    // Update is called once per frame
    void Update()
    { 
        PlayerController.instance.theSR.transform.rotation = new Quaternion(0, 0, 0, 1);
        PlayerController.instance.theRB.gravityScale = regularGravity;
        PlayerControl();
        SetCurrentGrapplePoint();
        DrawRope();
    }

    private void FixedUpdate()
    {
        Simulate();
    }

    private void SetCurrentGrapplePoint()
    {
        if (!playerInputActions.ActionMap.Grapple.IsPressed()) //!Input.GetButton("Grapple")
        {
            if (isPullGrapple)
            {
                //currentGrapplePoint.GetComponent<SpriteRenderer>().color = pullGrappleColour;
                currentGrapplePoint.GetComponent<SpriteRenderer>().sprite = pullGrappleOff;
            }
            else
            {
                //currentGrapplePoint.GetComponent<SpriteRenderer>().color = swingGrappleColour;
                currentGrapplePoint.GetComponent<SpriteRenderer>().sprite = swingGrappleOff;
            }

            for (int i = 0; i < swingGrapplePoints.Length; i++)
            {
                if (Vector2.Distance(swingGrapplePoints[i].transform.position, transform.position) < Vector2.Distance(currentGrapplePoint.transform.position, transform.position))
                {
                    currentGrapplePoint = swingGrapplePoints[i];
                    isPullGrapple = false;
                }
            }
            for (int i = 0; i < pullGrapplePoints.Length; i++)
            {
                if (Vector2.Distance(pullGrapplePoints[i].transform.position, transform.position) < Vector2.Distance(currentGrapplePoint.transform.position, transform.position))
                {
                    currentGrapplePoint = pullGrapplePoints[i];
                    isPullGrapple = true;
                }
            }

            if (Vector3.Distance(currentGrapplePoint.transform.position, transform.position) < maxGrappleDistance)
            {
                if (isPullGrapple)
                {
                    //currentGrapplePoint.GetComponent<SpriteRenderer>().color = pullGrappleColour / 1.5f;
                    currentGrapplePoint.GetComponent<SpriteRenderer>().sprite = pullGrappleOn;
                }
                else
                {
                    //currentGrapplePoint.GetComponent<SpriteRenderer>().color = swingGrappleColour / 1.5f;
                    currentGrapplePoint.GetComponent<SpriteRenderer>().sprite = swingGrappleOn;
                }
            }
        }
    }

    private void PlayerControl()
    {
        if (Vector3.Distance(transform.position, currentGrapplePoint.transform.position) < maxGrappleDistance)
        {
            if (playerInputActions.ActionMap.Grapple.WasPressedThisFrame() //Input.GetButtonDown("Grapple")// || (Input.GetButton("Grapple") && grapple.enabled == false))
            ){
                grapple.connectedBody = currentGrapplePoint.GetComponent<Rigidbody2D>();
                grapple.enabled = true;

                if (isPullGrapple)
                {
                    PlayerController.instance.theRB.gravityScale *= gravityReduction;
                }

                ropeSegLen = Vector3.Distance(transform.position, currentGrapplePoint.transform.position) * 0.3f / segmentLength;
                for (int i = 1; i < segmentLength; i++)
                {
                    RopeSegment firstSegment = ropeSegments[i];
                    firstSegment.posNow = (transform.position - currentGrapplePoint.transform.position).normalized * (i / segmentLength) + currentGrapplePoint.transform.position;
                    firstSegment.posOld = firstSegment.posNow;
                    ropeSegments[i] = firstSegment;
                }
                StartCoroutine(EnableRope());
            }

            if (playerInputActions.ActionMap.Grapple.IsPressed())// && grapple.isActiveAndEnabled) //Input.GetButton("Grapple")
            {
                if (grapple.isActiveAndEnabled)
                {
                    if (minGrappleDistance < Vector3.Distance(transform.position, currentGrapplePoint.transform.position)// && Vector3.Distance(transform.position, currentGrapplePoint.transform.position) < maxGrappleDistance)
                    )
                    {
                        if (!PlayerController.instance.isGrounded)
                        {
                            PlayerController.instance.theSR.transform.Rotate(Vector3.forward, -Vector2.SignedAngle(currentGrapplePoint.transform.position - transform.position, PlayerController.instance.theSR.transform.up));
                        }
                        if (isPullGrapple)
                        {
                            grapple.distance = Mathf.MoveTowards(grapple.distance, 0, grapplePullForce);
                            ropeSegLen = Vector3.Distance(transform.position, currentGrapplePoint.transform.position) * 0.3f / segmentLength;
                        }
                        //canGrapplePop = true;
                        previousDirection = (currentGrapplePoint.transform.position - transform.position).normalized;
                    }
                    else
                    {
                        transform.position = currentGrapplePoint.transform.position;
                        PlayerController.instance.theSR.transform.rotation = new Quaternion(0, 0, 0, 1);
                        PlayerController.instance.theRB.velocity = Vector2.zero;
                        rope.enabled = false;
                        StartCoroutine(CoyoteGrapplePop());
                    }
                }
                //PlayerController.instance.theRB.velocity = previousDirection * grapplePullForce;
            }
            //else
            //{
                if (playerInputActions.ActionMap.Grapple.WasReleasedThisFrame() && grapple.isActiveAndEnabled) //Input.GetButtonUp("Grapple")
                {
                    if (isPullGrapple// && canGrapplePop)
                   )
                    {
                        //PlayerController.instance.theRB.AddForce((currentGrapplePoint.transform.position - transform.position).normalized * grapplePullForce * grapplePop, ForceMode2D.Impulse);
                        //PlayerController.instance.theRB.velocity = PlayerController.instance.theRB.velocity.normalized * grapplePullForce;
                        PlayerController.instance.theRB.AddForce(previousDirection * grapplePullForce * 50 + previousDirection * grapplePop, ForceMode2D.Impulse);
                        grapple.distance = 10;
                        //PlayerController.instance.theRB.gravityScale /= gravityReduction;
                    }
                    PlayerController.instance.theRB.AddForce(PlayerController.instance.theRB.velocity.normalized * grapplePop, ForceMode2D.Impulse);
                }

            //}


        }

        if (!playerInputActions.ActionMap.Grapple.IsPressed())
        {
                grapple.enabled = false;
                rope.enabled = false;
        }
    }

    private void Simulate()
    {
        // SIMULATION
        Vector2 forceGravity = new Vector2(0f, -PlayerController.instance.theRB.gravityScale);

        for (int i = 1; i < segmentLength; i++)
        {
            RopeSegment firstSegment = ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
            ropeSegments[i] = firstSegment;
        }

        //CONSTRAINTS
        for (int i = 0; i < 100; i++)
        {
            ApplyConstraint();
        }
    }

    private void ApplyConstraint()
    {
        //Constrant to Mouse
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.posNow = currentGrapplePoint.transform.position; //Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ropeSegments[0] = firstSegment;

        RopeSegment lastSegment = ropeSegments[segmentLength - 1];
        lastSegment.posNow = transform.position;
        ropeSegments[segmentLength - 1] = lastSegment;

        for (int i = 0; i < segmentLength - 1; i++)
        {
            RopeSegment firstSeg = ropeSegments[i];
            RopeSegment secondSeg = ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if (dist > ropeSegLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < ropeSegLen)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                ropeSegments[i + 1] = secondSeg;
            }
        }
    }

    private void DrawRope()
    {
        float lineWidth = this.lineWidth;
        rope.startWidth = lineWidth;
        rope.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[segmentLength];
        for (int i = 0; i < segmentLength; i++)
        {
            ropePositions[i] = ropeSegments[i].posNow;
        }

        rope.positionCount = ropePositions.Length;
        rope.SetPositions(ropePositions);
    }

    private IEnumerator EnableRope()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        rope.enabled = true;
    }

    private IEnumerator CoyoteGrapplePop()
    {
        yield return new WaitForSeconds(PlayerController.instance.coyoteTime);
        //canGrapplePop = true;
        previousDirection = Vector2.zero;
    }

    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            posNow = pos;
            posOld = pos;
        }
    }
}
