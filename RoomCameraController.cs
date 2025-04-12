using UnityEngine;

public class RoomCameraController : MonoBehaviour
{
    public static RoomCameraController Instance;

    private float roomWidth;
    private float roomHeight;
    private Transform player;

    private void Awake()
    {
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        Camera cam = Camera.main;
        roomHeight = cam.orthographicSize * 2f;
        roomWidth = roomHeight * cam.aspect;
    }

    private void LateUpdate()
    {
        Vector3 targetPos = new Vector3(
            Mathf.Floor(player.position.x / roomWidth) * roomWidth + roomWidth / 2f,
            Mathf.Floor(player.position.y / roomHeight) * roomHeight + roomHeight / 2f,
            transform.position.z
        );

        transform.position = targetPos;
    }
}