using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float lifeTime = 2f;

    private Vector2 direcion;

    public void Initialize(Vector2 dir)
    {
        direcion = dir.normalized;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(direcion * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            Destroy(this.gameObject);
        }
    }
}
