using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    [SerializeField] ToggleState currentState = ToggleState.Active;

    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        ApplyState();
    }

    public void Trigger()
    {
        if(currentState == ToggleState.Toggled)
        {
            currentState = spriteRenderer.enabled ? ToggleState.Inactive : ToggleState.Active;
        }

        ApplyState();
    }
    
    private void ApplyState()
    {
        bool isVisible = (currentState == ToggleState.Active);
        if(spriteRenderer != null)
        {
            spriteRenderer.enabled = isVisible;
        }

        if(col != null)
        {
            col.enabled = isVisible;
        }
    }
}
