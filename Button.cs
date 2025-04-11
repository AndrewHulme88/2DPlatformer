using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] GameObject target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            target?.SendMessage("Activate");
        }
    }
}
