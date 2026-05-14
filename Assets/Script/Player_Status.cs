using System.Collections;
using UnityEngine;

public class Player_Status : MonoBehaviour
{
    public float maxHP = 10f;
    public float currentHP;

    public float attackPower = 1f;
    public float invincibleTime = 0.44f;
    bool isInvincible = false;

    void Start()
    {
        currentHP = maxHP;
    }

    // 체력 회복 (황금사과)
    public void AddHealth(float amount)
    {
        // 체력이 이미 꽉 차있으면 더하지 않음
        if (currentHP < maxHP)
        {
            currentHP += amount;

            // 만약 더했는데 최대 체력을 넘어버리면 최대치로 고정
            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }

            Debug.Log("사과 획득! 현재 체력: " + currentHP);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return;

        currentHP -= damage;
        Debug.Log("현재 체력: " + currentHP);

        if (currentHP <= 0)
        {
            Debug.Log("플레이어 사망!");
        }

        StartCoroutine(Invincible());
    }

    IEnumerator Invincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
}