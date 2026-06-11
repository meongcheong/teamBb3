using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float waitTime = 24.5f;   // 대기 시간
    public float fadeTime = 8f;    // 페이드 시간

    private SpriteRenderer sr;
    private float timer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 20초 전까지는 아무것도 안 함
        if (timer < waitTime)
            return;

        float fadeTimer = timer - waitTime;

        Color c = sr.color;
        c.a = Mathf.Lerp(1f, 0f, fadeTimer / fadeTime);
        sr.color = c;
    }
}