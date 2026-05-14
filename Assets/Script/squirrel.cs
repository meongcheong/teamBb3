using UnityEngine;

public class Squirrel : MonoBehaviour
{
    public float speed = 8.5f;
    public GameObject goldenApplePrefab;

    private Vector2 moveDirection;
    private bool droppedApple = false;
    private SquirrelSpawner spawner;

    void Start()
    {
        // 1. 화면에 있는 스포너 찾기
        spawner = FindFirstObjectByType<SquirrelSpawner>();

        // 2. 이동 방향 결정
        float randomY = Random.Range(-0.15f, 0.15f);

        if (transform.position.x < 0)
        {
            // 왼쪽에서 생성됨 -> 오른쪽으로 이동
            moveDirection = new Vector2(1, randomY);
        }
        else
        {
            // 오른쪽에서 생성됨 -> 왼쪽으로 이동
            moveDirection = new Vector2(-1, randomY);
        }

        // 방향 벡터를 정규화 (대각선이라고 더 빨라지지 않게 함)
        moveDirection = moveDirection.normalized;

        // 3. 몇 초 뒤에 사과 떨어뜨릴지 예약
        float randomDelay = Random.Range(0.6f, 2.2f);
        Invoke("DropApple", randomDelay);
    }

    void Update()
    {
        // 이동: 현재 위치 = 현재 위치 + (방향 * 속도 * 시간)
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;

        // Y축 이동 범위 제한 (화면 밖으로 나가지 않게)
        float currentY = transform.position.y;
        currentY = Mathf.Clamp(currentY, -4.01f, 0.21f);

        // 제한된 Y값을 다시 적용
        transform.position = new Vector3(transform.position.x, currentY, 0);

        // 화면 왼쪽이나 오른쪽으로 완전히 나가면 삭제
        if (transform.position.x < -12f || transform.position.x > 12f)
        {
            // 스포너에게 다람쥐가 사라졌다고 알려주기
            if (spawner != null)
            {
                spawner.ClearSquirrel();
            }

            Destroy(gameObject);
        }
    }

    void DropApple()
    {
        // 사과를 이미 떨어뜨렸으면 그냥 리턴
        if (droppedApple == true)
        {
            return;
        }

        // 사과 생성
        Instantiate(goldenApplePrefab, transform.position, Quaternion.identity);

        // 중복 방지 체크
        droppedApple = true;
    }
}