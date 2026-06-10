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

    public Slider hpSlider;
    public GameObject gameOverCanvas;

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        if (currentHP <= 0) return; // 사망 후 회복 방지

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
        if (isInvincible || !gameObject.activeInHierarchy || currentHP <= 0) return;

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
            audioManager.PlaySFX(audioManager.Dead);

            Die();
            return;
        }
        StartCoroutine(Invincible());
    }

    // 피해 입을 시 깜빡임 연출!
    IEnumerator Invincible()
    {
        isInvincible = true;
        audioManager.PlaySFX(audioManager.Attack, 0.2f);

        if (spriteRenderer != null)
        {
            // 피해 입을 시 빨간색으로 변경
            spriteRenderer.color = new Color(1f, 0.3f, 0.3f, 1f);
            yield return new WaitForSeconds(0.1f); // 0.1초 동안 유지

            // 남은 무적 시간동안 투명도를 조절하며 깜빡깜빡
            float timer = 0f;
            float blinkTargetTime = invincibleTime - 0.1f; // 나머지 시간 계산

            while (timer < blinkTargetTime)
            {
                // 투명도를 0.2와 1.0 사이로 빠르게 전환
                spriteRenderer.color = new Color(1f, 1f, 1f, spriteRenderer.color.a == 1f ? 0.2f : 1f);

                float blinkSpeed = 0.05f; // 깜빡이는 속도 (낮을수록 반짝임이 빨라짐)
                yield return new WaitForSeconds(blinkSpeed);
                timer += blinkSpeed;
            }

            // 무적이 끝나면 원래 상태로 되돌리기
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            // SpriteRenderer가 없을 때를 대비한 안전 예외 처리
            yield return new WaitForSeconds(invincibleTime);
        }

        isInvincible = false;
    }

    void Die()
    {
        StartCoroutine(DieRoutine());
    }

    IEnumerator DieRoutine()
    {
        Debug.Log("플레이어 사망! 사망 연출 시작");

        // 플레이어 조작 스크립트 끄기
        var moveScript = GetComponent("Player_Move") as MonoBehaviour;
        if (moveScript != null)
        {
            moveScript.enabled = false;
        }

        // 물리 속도 멈추기
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        // 쓰러지는 애니메이션 시시시작
        if (anim != null)
        {
            anim.SetTrigger("Dead");
        }

        // 애니메이션이 재생될 시간 동안 유니티 시간 멈추지 않고 기다리기
        yield return new WaitForSeconds(1.1f);

        // 애니메이션이 다 끝난 후에 게임 오버 창 띄우기
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }
      
        Time.timeScale = 0f; // 다 멈추기
    }
}