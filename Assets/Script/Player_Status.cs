using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player_Status : MonoBehaviour
{
    public float maxHP = 10f;
    public float currentHP;

    public float attackPower = 1f;
    public float invincibleTime = 0.44f;
    bool isInvincible = false;

    // HP 바
    public Slider hpSlider;
    public GameObject gameOverCanvas;

    void Start()
    {
        currentHP = maxHP;

        // 게임 시작할 때 체력바를 최대치로 설정
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
    }

    // 체력 회복 (황금사과)
    public void AddHealth(float amount)
    {
        if (currentHP < maxHP)
        {
            currentHP += amount;

            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }

            // 피가 차면 체력바 UI도 체력 참
            if (hpSlider != null)
            {
                hpSlider.value = currentHP;
            }

            Debug.Log("사과 획득! 현재 체력: " + currentHP);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return;

        currentHP -= damage;
        Debug.Log("현재 체력: " + currentHP);

        // 피가 깎였을땐 체력바 UI도 체력 줆
        if (hpSlider != null)
        {
            hpSlider.value = currentHP;
        }

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

    void Die()
    {
        Debug.Log("플레이어 사망! 게임 오버!");

        MonoBehaviour moveScript = GetComponent("Player_Move") as MonoBehaviour;
        if (moveScript != null)
        {
            moveScript.enabled = false;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }
    }
}