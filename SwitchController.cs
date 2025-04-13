using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public enum SwitchType { Toggle, Hold }

    [SerializeField] SwitchType switchType = SwitchType.Toggle;
    [SerializeField] ToggleObject[] targets;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite offSprite;
    [SerializeField] Sprite onSprite;
    [SerializeField] bool requiresButtonPress = false;
    [SerializeField] KeyCode interactionKey = KeyCode.LeftControl;

    private bool isOn = false;
    private bool playerIsInRange = false;

    private void Update()
    {
        if(requiresButtonPress && playerIsInRange && Input.GetKeyDown(interactionKey))
        {
            TriggerSwitch();
            UpdateSprite();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOn || !collision.CompareTag("Player")) return;

        playerIsInRange = true;

        if (!requiresButtonPress && switchType == SwitchType.Toggle)
        {
            isOn = !isOn;
            foreach (var target in targets)
            {
                target?.Toggle();
            }
        }
        else if (!requiresButtonPress && switchType == SwitchType.Hold)
        {
            isOn = true;
            foreach (var target in targets)
            {
                target?.Deactivate();
            }
        }

        UpdateSprite();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerIsInRange = false;

        if (!requiresButtonPress && switchType == SwitchType.Hold)
        {
            isOn = false;
            foreach (var target in targets)
            {
                target?.Activate();
            }

            UpdateSprite();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsInRange = true;
        }
    }

    private void TriggerSwitch()
    {
        isOn = !isOn;
        foreach(var target in targets)
        {
            target?.Toggle();
        }
    }

    private void UpdateSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = isOn ? onSprite : offSprite;
        }
    }
}
