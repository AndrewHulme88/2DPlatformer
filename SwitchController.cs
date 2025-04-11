using UnityEngine;

public class SwitchController : MonoBehaviour
{
    [SerializeField] ToggleObject[] targets;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        foreach (ToggleObject obj in targets)
        {
            obj.Trigger();
        }
    }
}
