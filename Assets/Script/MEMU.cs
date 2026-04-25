using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MEMU : MonoBehaviour
{
    public TMP_Dropdown graphicsDropdown;
    public Slider masterVol, MusicVol, sfxVol;
    public AudioMixer mainAudioMixer;

    public void ChangeGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
    }
    public void ChangeMasterVolume()
    {
        mainAudioMixer.SetFloat("MasterVol", masterVol.value);
    }
    public void ChangeMusicVolume()
    {
        mainAudioMixer.SetFloat("MusicVol", masterVol.value);
    }
    public void ChangeSfxVolume()
    {
        mainAudioMixer.SetFloat("SFXVol", masterVol.value);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
