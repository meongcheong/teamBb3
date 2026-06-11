using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player_Move : MonoBehaviour
{
    AudioManager audioManager;

    public float speed = 3f; // 속도
    public float dashSpeed = 10f; // 대시 속도
    public float dashTime = 0.2f; // 대시 길이
    public float dashCooldown = 5f; // 대시 쿨타임
    float dashTimer = 0f;
    bool isDashing = false;

    public Image dashUiImage;
    public Image skillUiImage;

    public Vector3 lastDir;
    float moveX;
    float moveY;

    public Vector3 inputVec;

    Player_Attack combat;
    SpriteRenderer spriter;
    Animator anim;

    private void Awake() // 초기화
    {
        combat = GetComponent<Player_Attack>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        if (dashTimer > 0) // 대시 UI
        {
            dashTimer -= Time.deltaTime;

            // 남은 대시 시간을 전체 쿨타임으로 나누어 1에서 0으로 줄어드는 비율을 만듦
            if (dashUiImage != null)
            {
                dashUiImage.fillAmount = dashTimer / dashCooldown;
            }
        }
        else
        {
            // 쿨타임이 완전히 끝나면 대시 아이콘을 다 찬 상태로 비워줌
            if (dashUiImage != null)
            {
                dashUiImage.fillAmount = 0f;
            }
        }

        // 독사과 UI
        if (combat != null && combat.skillTimer > 0)
        {
            if (skillUiImage != null)
            {
                skillUiImage.fillAmount = combat.skillTimer / combat.skillCooldown;
            }
        }
        else
        {
            if (skillUiImage != null)
            {
                skillUiImage.fillAmount = 0f; // 쿨타임이 아니면 다 찬 상태로 유지
            }
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

        // inputVec에 입력값을 넣어 공유
        inputVec = new Vector3(moveX, moveY, 0).normalized;

        if (inputVec != Vector3.zero)
        {
            lastDir = inputVec;
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

            Vector3 dir = inputVec;

            if (dir == Vector3.zero)
            {
                dir = lastDir;
            }

            audioManager.PlaySFX(audioManager.Dash,0.3f);

            StartCoroutine(Dash(dir));
            dashTimer = dashCooldown;
        }
    }

    void Move()
    {
        if (isDashing == true) return;
        transform.Translate(inputVec * speed * Time.deltaTime);
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
        // 애니메이터에 속도 값을 넘겨줌
        if (anim != null)
        {
            anim.SetFloat("Speed", inputVec.magnitude);
        }

        // 화면 밖으로 못 나가게 막기
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -8.27f, 8.25f);
        pos.y = Mathf.Clamp(pos.y, -3.91f, 0.21f);
        transform.position = pos;

       
        if (spriter != null)
        {
            if (inputVec.magnitude > 0)
            {
                if (moveX > 0)
                {
                    spriter.flipX = true; // 오른쪽 갈 때 원래 이미지 방향
                }
                else if (moveX < 0)
                {
                    spriter.flipX = false;  // 왼쪽 갈 때 이미지 뒤집기
                }
            }
        }
    }
}