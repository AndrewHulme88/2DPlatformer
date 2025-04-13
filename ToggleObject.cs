using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    private bool isActive = true;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    public void Activate()
    {
        SetActive(true);
    }

    public void Deactivate()
    {
        SetActive(false);
    }

    public void Toggle()
    {
        SetActive(!isActive);
    }

    private void SetActive(bool state)
    {
        isActive = state;
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = isActive;
        }
        if (col != null)
        {
            col.enabled = isActive;
        }
    }
}
