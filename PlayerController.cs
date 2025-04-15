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

    public bool canMove = true;
    public float startingGravity;
    public bool isFacingRight = true;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        startingGravity = rb.gravityScale;
    }

    private void Update()
    {
        if (!canMove) return;

        moveInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if(moveInput != 0)
        {
            isFacingRight = moveInput > 0;
            spriteRenderer.flipX = !isFacingRight;
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

        isClimbing = onLadder && Mathf.Abs(verticalInput) > 0;

        anim.SetFloat("moveX", Mathf.Abs(moveInput));
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
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
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
