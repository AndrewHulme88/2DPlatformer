using UnityEngine;

public class DigTool : MonoBehaviour
{
    [SerializeField] float hitDistance = 1f;
    [SerializeField] LayerMask breakableLayer;
    [SerializeField] KeyCode useKey = KeyCode.LeftAlt;
    [SerializeField] Transform breakOrigin;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(useKey))
        {
            TryBreak();
        }
    }

    private void TryBreak()
    {
        Vector2 direction = playerController.isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(breakOrigin.position, direction, hitDistance, breakableLayer);

        if(hit.collider != null)
        {
            Destroy(hit.collider.gameObject);
            Debug.Log("Hit " + hit.collider.name);
        }
    }
}
