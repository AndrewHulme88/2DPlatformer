using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Vector3 respawnPoint;

    private void Start()
    {
        respawnPoint = transform.position;
    }

    public void Die()
    {
        Debug.Log("Player died");
        transform.position = respawnPoint;
    }

    public void SetRespawnPoint(Vector3 newPoint)
    {
        respawnPoint = newPoint;
    }
}
