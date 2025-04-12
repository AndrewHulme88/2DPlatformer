using UnityEngine;

[ExecuteAlways]
public class RoomGridGizmo : MonoBehaviour
{
    public int gridWidth = 5;
    public int gridHeight = 5;
    public Vector2 origin = Vector2.zero;
    public Color lineColor = new Color(0, 1, 1, 0.2f);

    private void OnDrawGizmos()
    {
        Camera cam = Camera.main;
        if (cam == null || !cam.orthographic) return;

        float roomHeight = cam.orthographicSize * 2f;
        float roomWidth = roomHeight * cam.aspect;

        Gizmos.color = lineColor;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 center = new Vector3(
                    origin.x + (x + 0.5f) * roomWidth,
                    origin.y + (y + 0.5f) * roomHeight,
                    0
                );

                Vector3 size = new Vector3(roomWidth, roomHeight, 0);
                Gizmos.DrawWireCube(center, size);
            }
        }
    }
}
