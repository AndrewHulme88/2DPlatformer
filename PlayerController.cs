using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] float jumpBufferTime = 0.1f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.1f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] KeyCode aimLockKey = KeyCode.LeftShift;

    [HideInInspector] public bool isAimingLocked = false;

    public bool canMove = true;
    public float startingGravity;
    public bool isFacingRight = true;
    public Vector2 aimDirection = Vector2.right;
    public bool hasGun = false;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private float moveInput;
    private float jumpBufferTimer;
    private float coyoteTimer;
    private bool onLadder = false;
    private bool climbStarted = false;
    public bool isClimbing = false;
    private float verticalInput;
    private bool atLadderExit = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startingGravity = rb.gravityScale;
    }

    private void Update()
    {
        if (!canMove) return;

        moveInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isAimingLocked = Input.GetKey(aimLockKey);
        aimDirection.x = Input.GetAxisRaw("Horizontal");
        aimDirection.y = Input.GetAxisRaw("Vertical");
        int aimIndex = 0;

        isClimbing = (onLadder && Input.GetButton("Climb")) ? true : false;

        if(isClimbing && !climbStarted)
        {
            climbStarted = true;
        }
        else if(!isClimbing && climbStarted)
        {
            ResetClimbing();
        }

        if(isClimbing && atLadderExit)
        {
            if(verticalInput > 0 || verticalInput < 0)
            {
                isClimbing = false;
                ResetClimbing();
            }
        }

        if (moveInput != 0)
        {
            isFacingRight = moveInput > 0;
            Vector3 scale = transform.localScale;
            scale.x = isFacingRight ? 1 : -1;
            transform.localScale = scale;
        }

        if(isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Jump"))
        {
            jumpBufferTimer = jumpBufferTime;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        if(jumpBufferTimer > 0 && coyoteTimer > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferTimer = 0;
        }

        if(!isGrounded && verticalInput < 0)
        {
            aimDirection = new Vector2(moveInput, -1).normalized;
        }
        else if(verticalInput != 0 || moveInput != 0)
        {
            aimDirection = new Vector2(moveInput, verticalInput).normalized;
        }

        if(!isAimingLocked)
        {
            anim.SetFloat("moveX", Mathf.Abs(moveInput));
        }

        if(aimDirection.y > 0.5f && Mathf.Abs(aimDirection.x) > 0.5f)
        {
            aimIndex = 3; // Diagonal Up
        }
        else if(aimDirection.y < -0.5f && Mathf.Abs(aimDirection.x) > 0.5f)
        {
            aimIndex = 4; // Diagonal Down
        }
        else if(aimDirection.y > 0.5f)
        {
            aimIndex = 1; // Up
        }
        else if(aimDirection.y < -0.5f)
        {
            aimIndex = 2; // Down
        }
        else
        {
            aimIndex = 0; // Forward
        }

        if(aimDirection == Vector2.zero)
        {
            aimDirection = isFacingRight ? Vector2.right : Vector2.left;
        }

        anim.SetBool("isClimbing", isClimbing);
        anim.SetFloat("velocityY", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("hasGun", hasGun);
        anim.SetInteger("aimDirectionIndex", aimIndex);
    }

    private void FixedUpdate()
    {
        if(onLadder)
        {
            if(climbStarted && !isClimbing)
            {
                if (moveInput > 0.1f || moveInput < -0.1f)
                {
                    ResetClimbing();
                }
            }
            else if(climbStarted && isClimbing)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;

                Vector3 newPos = transform.position;
                newPos.y += verticalInput * climbSpeed * Time.deltaTime;
                rb.MovePosition(newPos);
            }
        }

        if(!isAimingLocked)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    private void ResetClimbing()
    {
        if(climbStarted)
        {
            climbStarted = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Ladder"))
        {
            onLadder = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("LadderExit"))
        {
            atLadderExit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("LadderExit"))
        {
            atLadderExit = false;
        }
        if(collision.CompareTag("Ladder"))
        {
            onLadder = false;
        }
    }
}
