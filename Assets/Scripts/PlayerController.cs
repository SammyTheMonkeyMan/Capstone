using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public Rigidbody2D theRB;

    public float moveSpeed;
    public float topSpeed;
    public float deceleration;

    public float jumpForce;
    public float jumpLength;
    private float jumpCounter;

    public float hopForce;

    public LayerMask whatIsGround;
    public Transform groundCheckPoint;
    public bool isGrounded;

    public Transform wallCheckPointL, wallCheckPointR;
    private bool isWalledL, isWalledR;

    public SpriteRenderer theSR;

    public float coyoteTime;
    private bool previousIsGrounded;
    private float previousVelocityX;

    //private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;

    //private bool canJump;

    public Animator anim;

    /*
    public float knockbackLength, knockbackForce, knockBackCounter;

    public float bounceForce;*/

    public bool stopControl;

    private void Awake()
    {
        instance = this;
        theRB = GetComponent<Rigidbody2D>();

    }

    // Start is called before the first frame update
    void Start()
    {
        // anim = GetComponent<Animator>();
        //theSR = GetComponent<SpriteRenderer>();
        //playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.ActionMap.Enable();
        //playerInputActions.ActionMap.Jump.performed += Jump;
        //Time.timeScale = 0.1f;
    }

    // Update is called once per frame

    void Update()
    {
        if (!PauseMenu.instance.paused && !stopControl)
        {
            //    if (knockBackCounter <= 0)
            //    {
            //if (isGrounded)
            //{
            //    theRB.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), theRB.velocity.y);
            //}

            if (Mathf.Abs(theRB.velocity.x) < topSpeed)
            {
                theRB.AddForce(Vector2.right * playerInputActions.ActionMap.Move.ReadValue<float>() * moveSpeed); //Input.GetAxisRaw("Horizontal")
            }

            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, 0.3f, whatIsGround);
            isWalledL = Physics2D.OverlapCircle(wallCheckPointL.position, 0.2f, whatIsGround);
            isWalledR = Physics2D.OverlapCircle(wallCheckPointR.position, 0.2f, whatIsGround);

            if (!isGrounded && previousIsGrounded)
            {
                StartCoroutine(CoyoteJump());
            }
            else
            {
                previousIsGrounded = isGrounded;
            }

            if (playerInputActions.ActionMap.Move.ReadValue<float>() != Mathf.Sign(theRB.velocity.x)) //Input.GetAxisRaw("Horizontal")
            {
                theRB.velocity = new Vector2(Mathf.MoveTowards(theRB.velocity.x, 0, deceleration), theRB.velocity.y);
            }

            if (theRB.velocity.x < 0)
            {
                theSR.flipX = true;
            }
            else if (theRB.velocity.x > 0)
            {
                theSR.flipX = false;
            }

            if (playerInputActions.ActionMap.Jump.WasPressedThisFrame() //Input.GetButtonDown("Jump"))
            )
            {
                if (previousIsGrounded)
                {
                    theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                    jumpCounter = jumpLength;
                    if (playerInputActions.ActionMap.Move.ReadValue<float>() == -Mathf.Sign(theRB.velocity.x))
                    {
                        theRB.AddForce(Vector2.right * Mathf.Sign(theRB.velocity.x) * jumpForce * hopForce);
                    }
                }
                else if (isWalledL)
                {
                    theRB.velocity = new Vector2(jumpForce - previousVelocityX, jumpForce);
                    jumpCounter = jumpLength;
                }
                else if (isWalledR)
                {
                    theRB.velocity = new Vector2(-jumpForce - previousVelocityX, jumpForce);
                    jumpCounter = jumpLength;
                }
                AudioManager.instance.PlaySFX("Player Jump");
            }
            if (playerInputActions.ActionMap.Jump.IsPressed() && !isGrounded && jumpCounter > 0 //Input.GetButton("Jump"))
            )
            {
                theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                jumpCounter -= Time.deltaTime;
            }
            if (playerInputActions.ActionMap.Jump.WasReleasedThisFrame() //Input.GetButtonUp("Jump"))
            )
            {
                jumpCounter = 0;
            }

            if (Mathf.Abs(theRB.velocity.x) < 0.1f)
            {
                StartCoroutine(CoyoteWallJump());
            }
            else
            {
                previousVelocityX = theRB.velocity.x;
            }

            //    }
            //    else
            //    {
            //        knockBackCounter -= Time.deltaTime;
            //        if (theSR.flipX)
            //        {
            //            theRB.velocity = new Vector2(knockbackForce, theRB.velocity.y);
            //        }
            //        else
            //        {
            //            theRB.velocity = new Vector2(-knockbackForce, theRB.velocity.y);
            //        }
            //    }
            // }

            anim.SetBool("isGrounded", isGrounded);
            anim.SetFloat("moveSpeed", Mathf.Abs(theRB.velocity.x));
        }
    }

    private IEnumerator CoyoteJump()
    {
        yield return new WaitForSeconds(coyoteTime);
        previousIsGrounded = isGrounded;
    }

    private IEnumerator CoyoteWallJump()
    {
        yield return new WaitForSeconds(coyoteTime);
        previousVelocityX = theRB.velocity.x;
    }
    /*
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started//Input.GetButtonDown("Jump"))
            )
        {
            if (previousIsGrounded)
            {
                theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                jumpCounter = jumpLength;
            }
            else if (isWalledL)
            {
                theRB.velocity = new Vector2(jumpForce - previousVelocityX, jumpForce);
                jumpCounter = jumpLength;
            }
            else if (isWalledR)
            {
                theRB.velocity = new Vector2(-jumpForce - previousVelocityX, jumpForce);
                jumpCounter = jumpLength;
            }
            //AudioManager.instance.PlaySFX("Player Jump");
        }
        if (context.performed && !isGrounded && jumpCounter > 0 //Input.GetButton("Jump"))
            )
        {
            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
            jumpCounter -= Time.deltaTime;
        }
        if (context.canceled //Input.GetButtonUp("Jump"))
            )
        {
            jumpCounter = 0;
        }
    }*/
    /*public void KnockBack()
    {
        knockBackCounter = knockbackLength;
        theRB.velocity = new Vector2(theRB.velocity.x, knockbackForce);
    }

    public void Bounce()
    {
        theRB.velocity = new Vector2(theRB.velocity.x, bounceForce);
        AudioManager.instance.PlaySFX("Player Jump");
    }*/

}

