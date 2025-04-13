using System.Collections;
using UnityEngine;

public class TimedBlock : MonoBehaviour
{
    [SerializeField] float onTime = 2f;
    [SerializeField] float offTime = 2f;
    [SerializeField] bool startActive = true;
    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;

    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        if(startActive)
        {
            StartCoroutine(CycleOn());
        }
        else
        {
            StartCoroutine(CycleOff());
        }
    }

    private IEnumerator CycleOn()
    {
        SetState(true);
        yield return new WaitForSeconds(onTime);
        StartCoroutine(CycleOff());
    }

    private IEnumerator CycleOff()
    {
        SetState(false);
        yield return new WaitForSeconds(offTime);
        StartCoroutine(CycleOn());
    }

    private void SetState(bool active)
    {
        col.enabled = active;

        if(active)
        {
            spriteRenderer.sprite = onSprite;
        }
        else
        {
            spriteRenderer.sprite = offSprite;
        }
    }
}
