using UnityEngine;

public class PoisonApple : MonoBehaviour
{
    Vector2 direction;

    public float speed = 2f;
    public float lifeTime = 3f;

    Rigidbody2D rigid;

    public void SetDirection(Vector3 dir)
    {
        if (dir == Vector3.zero)
            dir = Vector3.right;

        direction = dir.normalized;
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.linearVelocity = direction * speed;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {

    }

    bool hasHit = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (hasHit) return; // 이미 맞았으면 무시

        // 보스 태그를 가진 오브젝트와 부딪혔을 때
        if (col.CompareTag("Boss"))
        {
            hasHit = true; // 한 번만 맞게 제한

            BossDwarf boss = col.GetComponent<BossDwarf>();
            

            if (boss != null)
            {
              
                // 보스 스크립트에게 독사과 맞은 신호 보냄
                boss.HitByPoisonApple();
            }

            Destroy(gameObject);
        }
    }
}