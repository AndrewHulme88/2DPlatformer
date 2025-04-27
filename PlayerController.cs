using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string CurrentRoom { get; set; }

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] float jumpBufferTime = 0.1f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.1f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] KeyCode aimLockKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouchKey = KeyCode.DownArrow;
    [SerializeField] BoxCollider2D standingCollider;
    [SerializeField] CapsuleCollider2D crouchingCollider;

    [HideInInspector] public bool isAimingLocked = false;

    public bool canMove = true;
    public float startingGravity;
    public bool isFacingRight = true;
    public Vector2 aimDirection = Vector2.right;
    public bool isCrouching = false;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private float moveInput;
    private float jumpBufferTimer;
    private float coyoteTimer;
    private float verticalInput;
    private MovingPlatform movingPlatform;

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
        isCrouching = Input.GetKey(crouchKey) && isGrounded && moveInput == 0;

        standingCollider.enabled = !isCrouching;
        crouchingCollider.enabled = isCrouching;

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
        else if (aimDirection.y < -0.5f && !isGrounded)
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

        if(isCrouching)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            aimDirection = isFacingRight ? Vector2.right : Vector2.left;
            aimIndex = 0;
        }

        anim.SetFloat("velocityY", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetInteger("aimDirectionIndex", aimIndex);
        anim.SetBool("isCrouching", isCrouching);
    }

    private void FixedUpdate()
    {
        if(!isAimingLocked && !isCrouching)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        if (movingPlatform != null)
        {
            rb.position += (Vector2)movingPlatform.PlatformVelocity * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("MovingPlatform"))
        {
            movingPlatform = collision.GetComponent<MovingPlatform>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("MovingPlatform"))
        {
            movingPlatform = null;
        }
    }
}
