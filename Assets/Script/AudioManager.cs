using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioMixerGroup sfxGroup;

    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip Dead;
    public AudioClip Dash;
    public AudioClip Golden;
    public AudioClip Poision;
    public AudioClip Squirrel;
    public AudioClip UI;
    public AudioClip Hit;
    public AudioClip RED; // 테스트용으로 넣은거라 나중에 지워야함!!


    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        SFXSource.PlayOneShot(clip);
    }
    public void PlaySFX(AudioClip clip, float startTime)
    {
        if (clip == null) return;

        SFXSource.clip = clip;
        SFXSource.time = startTime;

        SFXSource.Play();
    }
}

