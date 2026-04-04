using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    Player_Status status;

    public float skillCooldown = 15f;
    float skillTimer = 0f;

    void Start()
    {
        status = GetComponent<Player_Status>();
    }

    void Update()
    {
        if (skillTimer > 0)
            skillTimer -= Time.deltaTime;
    }

    // 평타
    public void Attack()
    {
        float damage = GetAttackPower();
        Debug.Log("평타 공격 / 데미지: " + damage);
    }

    // 독사과
    public void UseSkill()
    {
        if (skillTimer > 0)
        {
            Debug.Log("스킬 쿨타임 중");
            return;
        }

        Debug.Log("독사과 사용!");
        skillTimer = skillCooldown;
    }

    float GetAttackPower()
    {
        return status.attackPower * (status.currentHP / status.maxHP);
    }
}