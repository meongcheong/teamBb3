using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    Player_Status status;
    Player_Move move;

    public GameObject PoisonApplePrefab;
    public float PoisonAppleSpeed;

    public float skillCooldown = 15f;
    float skillTimer = 0f;

    void Start()
    {
        status = GetComponent<Player_Status>();
        move = GetComponent<Player_Move>();
    }

    void Update()
    {
        if (skillTimer > 0)
            skillTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E))
        {
            UseSkill();
        }
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
        Vector3 dir = move.lastDir;

        GameObject skill = Instantiate(PoisonApplePrefab, transform.position, Quaternion.identity);
        skill.GetComponent<PoisonApple>().SetDirection(dir);

        skillTimer = skillCooldown;
        Debug.Log("독사과 사용!");

    }

    float GetAttackPower()
    {
        return status.attackPower * (status.currentHP / status.maxHP);
    }
}