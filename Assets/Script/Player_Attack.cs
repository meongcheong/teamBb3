using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    Player_Status status;
    Player_Move move;
    AudioManager audioManager;

    public GameObject PoisonApplePrefab;
    public float PoisonAppleSpeed;

    public float skillCooldown = 15.0f;
    public float skillTimer = 0f;

    // 평타 연속 공격을 막기 위한 쿨타임 (0.56초)
    public float attackCooldown = 0.56f;
    public float attackTimer = 0f;

    // 59x56 크기의 충돌 상자를 담을 변수
    public GameObject attackArea;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        status = GetComponent<Player_Status>();
        move = GetComponent<Player_Move>();

        if (attackArea != null) attackArea.SetActive(false);
    }

    void Update()
    {
        // 독사과 쿨타임 계산
        if (skillTimer > 0)
            skillTimer -= Time.deltaTime;

        // 평타 쿨타임 계산
        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
    }

    // 평타
    public void Attack()
    {
        // 평타 쿨타임 중이면 공격 불가!
        if (attackTimer > 0) return;

        float damage = GetAttackPower();
        audioManager.PlaySFX(audioManager.Hit,0.1f);

        if (attackArea != null)
        {
            attackArea.SetActive(true);
            Invoke("DisableAttackArea", 0.7f);
        }

        // 공격했으니 평타 타이머 작동
        attackTimer = attackCooldown;
    }

    void DisableAttackArea()
    {
        if (attackArea != null) attackArea.SetActive(false);
    }

    // 독사과
    public void UseSkill()
    {
        if (skillTimer > 0)
        {
            Debug.Log("스킬 쿨타임 중");
            return;
        }
        Vector3 dir = move.lastDir;
        audioManager.PlaySFX(audioManager.Poision);

        GameObject skill = Instantiate(PoisonApplePrefab, transform.position, Quaternion.identity);
        skill.GetComponent<PoisonApple>().SetDirection(dir);

        skillTimer = skillCooldown;
        Debug.Log("독사과 사용!");
    }

    public float GetAttackPower()
    {
        if (status == null) return 0f;
        return status.attackPower;
    }
}