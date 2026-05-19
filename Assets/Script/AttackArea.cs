using UnityEngine;

public class AttackArea : MonoBehaviour
{
    public Player_Attack playerAttack; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            BossDwarf boss = collision.GetComponent<BossDwarf>();

            // 보스가 존재하고, 독사과에 맞은 상태
            if (boss != null && boss.isPoisoned == true)
            {
                // 플레이어의 평타 데미지를 계산
                float damage = playerAttack.GetAttackPower();

                // 보스에게 평타 데미지
                boss.TakeDamage(damage);
                Debug.Log("보스에게 평타 성공! 데미지: " + damage);
            }
            else
            {
                Debug.Log("보스가 독사과를 맞지 않은 상태라 평타가 통하지 않습니다!");
            }
        }
    }
}