using UnityEngine;

public class Player_Move : MonoBehaviour
{
    public Vector3 lastDir;
    public float speed = 3f;

    float moveX;
    float moveY;

    bool isDashing = false;
    public float dashSpeed = 10f;
    public float dashTime = 0.2f;

    float dashCooldown = 2f;
    float dashTimer = 0f;

    Player_Attack combat;

    void Start()
    {
        combat = GetComponent<Player_Attack>();
    }

    void Update()
    {
        if (dashTimer > 0)
            dashTimer -= Time.deltaTime;

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


        Vector3 inputDir = new Vector3(moveX, moveY, 0);

        if (inputDir != Vector3.zero)
        {
            lastDir = inputDir.normalized;
        }

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
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isDashing)
                return;

            if (dashTimer > 0)
            {
                Debug.Log("대시 쿨타임 중");
                return;
            }

            Vector3 dir = new Vector3(moveX, moveY, 0).normalized;

            if (dir == Vector3.zero)
                dir = lastDir;

            StartCoroutine(Dash(dir));
            dashTimer = dashCooldown;
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

    // 경계 설정
    void LateUpdate()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, -8.27f, 8.25f);
        pos.y = Mathf.Clamp(pos.y, -3.91f, 0.21f);

        transform.position = pos;
    }

}