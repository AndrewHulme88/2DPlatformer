using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyFlyingPatroller : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float pauseTime = 0.5f;
    [SerializeField] List<Transform> patrolPoints;

    private int currentIndex = 0;
    private bool isPaused = false;

    private void Update()
    {
        if (isPaused || patrolPoints.Count == 0) return;

        Transform target = patrolPoints[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            StartCoroutine(SwitchToNextPoint());
        }
    }

    private IEnumerator SwitchToNextPoint()
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseTime);
        currentIndex = (currentIndex + 1) % patrolPoints.Count;

        if(patrolPoints.Count > 1)
        {
            float direction = patrolPoints[currentIndex].position.x - transform.position.x;
            Vector3 scale = transform.localScale;
            scale.x = direction > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        isPaused = false;
    }
}
