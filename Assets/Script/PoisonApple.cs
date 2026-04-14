using UnityEngine;

public class PoisonApple : MonoBehaviour
{
    Vector2 direction;

    public float speed = 3f;
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

    }

    bool hasHit = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (hasHit) return; // 이미 맞았으면 무시

        if (col.CompareTag("Boss"))
        {
            hasHit = true; //  한 번만 맞게 제한

            BossDwarf boss = col.GetComponent<BossDwarf>();

            if (boss != null)
            {
                boss.TakeDamage(2f);
            }

            Destroy(gameObject);
        }
    }



}
