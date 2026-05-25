using System.Collections;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    public float speed = 3f; // 속도
    public float dashSpeed = 10f; // 대시 속도
    public float dashTime = 0.2f; // 대시 길이
    public float dashCooldown = 2f; // 대시 쿨타임

    public Sprite imgUp;    // 뒷모습
    public Sprite imgDown;  // 앞모습
    public Sprite imgRight; // 옆모습

    public Vector3 lastDir;
    float moveX;
    float moveY;
    float dashTimer = 0f;
    bool isDashing = false;

    // 마지막으로 누른 방향이 어디였는지 글자로 기억
    // 처음에 가만히 있을 때는 앞모습이 디폴트니까 기본값은 Down
    string lastVerticalState = "Down";

    Player_Attack combat;
    SpriteRenderer spriter;

    void Awake()
    {
        combat = GetComponent<Player_Attack>();
        spriter = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }

        InputCheck();
        Move();
    }

    void InputCheck()
    {
        moveX = 0;
        moveY = 0;

        if (Input.GetKey(KeyCode.W)) moveY = 1;
        if (Input.GetKey(KeyCode.S)) moveY = -1;
        if (Input.GetKey(KeyCode.A)) moveX = -1;
        if (Input.GetKey(KeyCode.D)) moveX = 1;

        // 마지막으로 누른 방향을 글자로 저장
        if (Input.GetKeyDown(KeyCode.W)) lastVerticalState = "Up";
        if (Input.GetKeyDown(KeyCode.S)) lastVerticalState = "Down";

        Vector3 inputDir = new Vector3(moveX, moveY, 0);

        if (inputDir != Vector3.zero)
        {
            lastDir = inputDir.normalized;
        }

        if (Input.GetMouseButtonDown(0))
        {
            combat.Attack();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            combat.UseSkill();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isDashing == true) return;
            if (dashTimer > 0) return;

            Vector3 dir = inputDir.normalized;

            if (dir == Vector3.zero)
            {
                dir = lastDir;
            }

            StartCoroutine(Dash(dir));
            dashTimer = dashCooldown;
        }
    }

    void Move()
    {
        if (isDashing == true) return;

        Vector3 move = new Vector3(moveX, moveY, 0).normalized;
        transform.Translate(move * speed * Time.deltaTime);
    }

    IEnumerator Dash(Vector3 dir)
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

    void LateUpdate()
    {
        // 화면 밖으로 못 나가게 막기
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -8.27f, 8.25f);
        pos.y = Mathf.Clamp(pos.y, -3.91f, 0.21f);
        transform.position = pos;

        // 이동 방향에 따른 이미지 처리
        if (spriter != null)
        {
            // 위나 아래로만 움직이고 있을 때
            if (moveY > 0)
            {
                spriter.sprite = imgUp;
                if (moveX > 0) spriter.flipX = false;
                else if (moveX < 0) spriter.flipX = true;
            }
            else if (moveY < 0)
            {
                spriter.sprite = imgDown;
                if (moveX > 0) spriter.flipX = false;
                else if (moveX < 0) spriter.flipX = true;
            }
            // 좌우키만 단독으로 누르고 있을 때
            else if (moveX != 0)
            {
                // 마지막으로 누른 키가 W였다면? -> 뒷모습으로 플립
                if (lastVerticalState == "Up")
                {
                    spriter.sprite = imgUp;

                    if (moveX > 0) spriter.flipX = false; // D 누르면 정방향 뒷모습
                    else if (moveX < 0) spriter.flipX = true; // A 누르면 뒤집힌 뒷모습
                }
                // 마지막으로 누른 키가 S였다면 -> 앞모습으로 플립
                else if (lastVerticalState == "Down")
                {
                    spriter.sprite = imgDown;

                    if (moveX > 0) spriter.flipX = true; // D 누르면 정방향 앞모습
                    else if (moveX < 0) spriter.flipX = false; // A 누르면 뒤집힌 앞모습
                }
            }
        }
    }
}