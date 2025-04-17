using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] float speed = 2f;

    private Vector3 target;

    public Vector3 PlatformVelocity { get; private set; }

    private void Start()
    {
        target = pointB.position;
    }

    private void Update()
    {
        Vector3 newPosition = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        PlatformVelocity = (newPosition - transform.position) / Time.deltaTime;
        transform.position = newPosition;

        if(Vector3.Distance(transform.position, target) < 0.05f)
        {
            target = (target == pointA.position) ? pointB.position : pointA.position;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.CompareTag("Player"))
    //    {
    //        Transform player = collision.transform;
    //        Vector3 originalScale = player.localScale;
    //        player.SetParent(transform);
    //        player.localScale = originalScale;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if(collision.CompareTag("Player"))
    //    {
    //        collision.transform.SetParent(null);
    //    }
    //}
}
