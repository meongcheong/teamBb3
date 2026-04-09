using UnityEngine;

public class PoisonApple : MonoBehaviour
{
    Vector2 direction;

    public float speed = 5f;
    public float lifeTime = 3f;

    Rigidbody2D rigid;

    public void SetDirection(Vector3 dir)
    {
        if (dir == Vector3.zero)
            dir = Vector3.right;

        direction = dir.normalized;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.linearVelocity = direction* speed;

        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        rigid.linearVelocity = direction * speed;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
}
