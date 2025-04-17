using UnityEngine;
using System.Collections;

public class EnemyWallPatroller : MonoBehaviour
{
    public enum WallSide { Right, Left }

    [SerializeField] float moveSpeed = 2f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckDistance = 0.1f;
    [SerializeField] Transform wallCheck;
    [SerializeField] float wallCheckDistance = 0.5f;
    [SerializeField] float pauseTime = 0.5f;
    [SerializeField] WallSide wallSide = WallSide.Right;

    private Rigidbody2D rb;
    private bool isMovingUp = true;
    private bool isTurning = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        if(wallSide == WallSide.Left)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void FixedUpdate()
    {
        if (isTurning) return;

        rb.linearVelocity = new Vector2(0, (isMovingUp ? 1 : -1) * moveSpeed);
        Vector2 wallNormal = (wallSide == WallSide.Right) ? Vector2.right : Vector2.left;
        RaycastHit2D groundHit = Physics2D.Raycast(groundCheck.position, wallNormal, groundCheckDistance, groundLayer);
        RaycastHit2D wallHit = Physics2D.Raycast(wallCheck.position, isMovingUp ? Vector2.up : Vector2.down, wallCheckDistance, groundLayer);

        if (!groundHit.collider || wallHit.collider)
        {
            StartCoroutine(FlipAfterPause());
        }
    }

    private IEnumerator FlipAfterPause()
    {
        isTurning = true;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(pauseTime);
        Flip();
        isTurning = false;
    }

    private void Flip()
    {
        isMovingUp = !isMovingUp;
        Vector3 scale = transform.localScale;
        scale.y *= -1;

        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.right * groundCheckDistance);

        if (wallCheck != null)
        {
            Vector3 dir = isMovingUp ? Vector3.up : Vector3.down;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + dir * wallCheckDistance);
        }
    }
}
