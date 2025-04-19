using System.Collections;
using UnityEngine;

public class EnemyFloater : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] float moveRadius = 3f;
    [SerializeField] float pauseTime = 1f;

    private Vector2 startPos;
    private Vector2 targetPos;
    private bool isMoving = false;

    private void Start()
    {
        startPos = transform.position;
        StartCoroutine(ChooseNewTarget());
    }

    private void Update()
    {
        if (!isMoving) return;

        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if(Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            isMoving = false;
            StartCoroutine(ChooseNewTarget());
        }
    }

    private IEnumerator ChooseNewTarget()
    {
        yield return new WaitForSeconds(pauseTime);

        Vector2 randomOffset = Random.insideUnitCircle * moveRadius;
        targetPos = startPos + randomOffset;
        isMoving = true;
        Vector3 scale = transform.localScale;
        scale.x = targetPos.x > transform.position.x ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) return;

        StartCoroutine(ChooseNewTarget());
    }
}
