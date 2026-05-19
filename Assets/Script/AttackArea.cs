using UnityEngine;

public class AttackArea : MonoBehaviour
{
    public Player_Attack playerAttack; // 플레이어 공격 스크립트 연결용

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 부딪힌 대상이 보스("Boss")라면
        if (collision.CompareTag("Boss"))
        {
            // 보스의 BossDwarf 스크립트를 가져옵니다.
            BossDwarf boss = collision.GetComponent<BossDwarf>();

            // 보스가 존재하고, 독사과에 맞은 상태(isPoisoned가 true)일 때만!
            if (boss != null && boss.isPoisoned == true)
            {
                // 플레이어의 평타 데미지를 계산해 가져옵니다 (이름을 GetAttackPower로 매칭!)
                float damage = playerAttack.GetAttackPower();

                // 보스에게 평타 데미지를 입힙니다!
                boss.TakeDamage(damage);
                Debug.Log("보스에게 평타 성공! 데미지: " + damage);

                // (선택 사항) 평타를 한 번 때리면 독 효과가 바로 사라지게 하고 싶다면 아래 주석을 푸세요!
                // boss.isPoisoned = false;
            }
            else
            {
                // 독사과를 안 맞은 상태라면 평타가 들어가지 않습니다.
                Debug.Log("보스가 독사과를 맞지 않은 상태라 평타가 통하지 않습니다!");
            }
        }
    }
}