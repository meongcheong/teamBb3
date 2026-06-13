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
    public AudioClip Attack;

    public bool isCutScenePlaying = false;

    private void Start()
    {
        if (isCutScenePlaying == true) return;

        musicSource.clip = background;
        musicSource.Play();
    }

    public void SetCutScenePlaying(bool active)
    {
        isCutScenePlaying = active;

        if (isCutScenePlaying == true)
        {
            StopBackgroundBGM();
        }
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

    public void StopBackgroundBGM()
    {
        if (musicSource == null) return;

        musicSource.Stop();
    }

    public void PlayBackgroundBGM()
    {

        if (isCutScenePlaying == true)
        {
            return;
        }

        if (musicSource == null || background == null) return;
        Debug.Log("BGM 재생 시도");
        musicSource.clip = background;
        musicSource.Play();
    }
}