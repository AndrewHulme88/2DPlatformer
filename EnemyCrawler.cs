using UnityEngine;

public class EnemyCrawler : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float checkDistance = 0.1f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] bool moveClockwise = true;

    private float rotateCooldown = 0.2f;
    private float lastRotateTime = 0f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if(!moveClockwise)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        float direction = moveClockwise ? 1f : -1f;
        float originDirection = moveClockwise ? 0.5f : -0.5f;

        transform.Translate(Vector2.right * direction * moveSpeed * Time.fixedDeltaTime);

        Vector2 origin = (Vector2)transform.position + (Vector2)(transform.right * originDirection);
        RaycastHit2D groundCheck = Physics2D.Raycast(origin, -transform.up, checkDistance, groundLayer);

        if (!groundCheck && Time.time > lastRotateTime + rotateCooldown)
        {
            RotateAroundCorner();
            lastRotateTime = Time.time;
        }
    }

    private void RotateAroundCorner()
    {
        float rotationAmount = moveClockwise ? -90f : 90f;
        transform.Rotate(0, 0, rotationAmount);
        Vector3 offset = transform.up * 0.5f;
        transform.position += offset;
    }
}
