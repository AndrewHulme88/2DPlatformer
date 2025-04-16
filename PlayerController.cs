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

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private float moveInput;
    private float jumpBufferTimer;
    private float coyoteTimer;
    private bool onLadder = false;
    public bool isClimbing = false;
    private float verticalInput;

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

        if(moveInput != 0)
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

        isClimbing = onLadder && Mathf.Abs(verticalInput) > 0;

        if(!isAimingLocked)
        {
            anim.SetFloat("moveX", Mathf.Abs(moveInput));
        }

        anim.SetFloat("velocityY", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isClimbing", onLadder);
    }

    private void FixedUpdate()
    {
        if(onLadder)
        { 
            rb.gravityScale = 0;

            if(Mathf.Abs(verticalInput) > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalInput * climbSpeed);
            }
            else
            {
                rb.linearVelocity = new Vector2(moveInput * moveSpeed, 0);
            }
        }
        else
        {
            rb.gravityScale = startingGravity;

            if(!isAimingLocked)
            {
                rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ladder"))
        {
            onLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Ladder"))
        {
            onLadder = false;
        }
    }
}
