using UnityEngine;

public class GoldenApple : MonoBehaviour
{
    void Start()
    {
        // !!!!!!!!!!!!!!!!!!테스트 다 하고 난 뒤에 >>> 10초 뒤 삭제로 변경해야됨
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 부딪힌 물체의 태그가 Player라면
        if (other.CompareTag("Player"))
        {
            // 플레이어의 Player_Status 스크립트를 가져옴
            Player_Status status = other.GetComponent<Player_Status>();

            if (status != null)
            {
                // 체력을 1만큼 회복시킴
                status.AddHealth(1f);
            }

            // 사과 파괴
            Destroy(gameObject);
        }
    }
}
