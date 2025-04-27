using Unity.Cinemachine;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] CompositeCollider2D cameraBounds;
    [SerializeField] Vector2 newCameraPosition;
    [SerializeField] string roomName;
    [SerializeField] GameObject roomContents;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();

            if(playerController != null && playerController.CurrentRoom != this.roomName)
            {
                RoomManager.Instance.SetActiveRoom(roomContents.name);

                var confiner = FindFirstObjectByType<CinemachineConfiner2D>();

                if(confiner != null)
                {
                    confiner.BoundingShape2D = cameraBounds;
                    confiner.InvalidateBoundingShapeCache();

                }

                var vcam = FindFirstObjectByType<CinemachineCamera>();

                if(vcam != null)
                {
                    vcam.ForceCameraPosition(newCameraPosition, vcam.transform.rotation);
                }

                playerController.CurrentRoom = this.roomName;
            }
        }
    }
}
