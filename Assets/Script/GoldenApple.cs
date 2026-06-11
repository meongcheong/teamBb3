using System.Collections;
using UnityEngine;

public class GoldenApple : MonoBehaviour
{
    AudioManager audioManager;
    SpriteRenderer sr;

    public float lifeTime = 7f;
    public float blinkStartTime = 5f;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        StartCoroutine(LifeRoutine());
    }

    IEnumerator LifeRoutine()
    {
        float t = 0f;
        float blinkInterval = 0.15f;
        float blinkTimer = 0f;
        bool visible = true;

        while (t < lifeTime)
        {
            t += Time.deltaTime;

            // 마지막 3초부터 깜빡임
            if (t >= blinkStartTime)
            {
                blinkTimer += Time.deltaTime;

                if (blinkTimer >= blinkInterval)
                {
                    blinkTimer = 0f;
                    visible = !visible;

                    if (sr != null)
                    {
                        Color c = sr.color;
                        c.a = visible ? 1f : 0.3f;
                        sr.color = c;
                    }
                }
            }

            yield return null;
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player_Status status = other.GetComponent<Player_Status>();

            if (status != null)
            {
                audioManager.PlaySFX(audioManager.Golden);
                status.AddHealth(1f);
            }

            Destroy(gameObject);
        }
    }
}