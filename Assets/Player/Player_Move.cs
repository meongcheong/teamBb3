using UnityEngine;

public class Player_Move : MonoBehaviour
{
    public float speed = 5f;

    float moveX;
    float moveY;

    bool isDashing = false;
    public float dashSpeed = 10f;
    public float dashTime = 0.2f;

    Player_Attack combat;

    void Start()
    {
        combat = GetComponent<Player_Attack>();
    }

    void Update()
    {
        InputCheck();  // 입력 처리
        Move();        // 이동 처리
    }

    void InputCheck()
    {
        moveX = 0;
        moveY = 0;

        if (Input.GetKey(KeyCode.W)) moveY += 1;
        if (Input.GetKey(KeyCode.S)) moveY -= 1;
        if (Input.GetKey(KeyCode.A)) moveX -= 1;
        if (Input.GetKey(KeyCode.D)) moveX += 1;

        // 평타
        if (Input.GetMouseButtonDown(0))
        {
            combat.Attack();
        }

        // 스킬
        if (Input.GetKeyDown(KeyCode.E))
        {
            combat.UseSkill();
        }

        // 대시
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            Vector3 dir = new Vector3(moveX, moveY, 0).normalized;
            StartCoroutine(Dash(dir));
        }
    }

    void Move()
    {
        if (isDashing) return;

        Vector3 move = new Vector3(moveX, moveY, 0).normalized;
        transform.Translate(move * speed * Time.deltaTime);
    }

    System.Collections.IEnumerator Dash(Vector3 dir)
    {
        isDashing = true;

        float timer = 0f;
        while (timer < dashTime)
        {
            transform.Translate(dir * dashSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }
}