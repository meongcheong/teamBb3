using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float fadeTime = 8f;

    private SpriteRenderer sr;
    private float timer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        Color c = sr.color;
        c.a = Mathf.Lerp(1f, 0f, timer / fadeTime);

        sr.color = c;
    }
}