using System.Collections.Generic;
using System.Security.Cryptography;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class Quality : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Toggle windowedToggle;

    List<Resolution> resolutions = new List<Resolution>();

    int selectedIndex;
    bool selectedwindowed;

    void Start()
    {
        resolutions.Clear();
        resolutions.Add(new Resolution { width = 1920, height = 1080 });
        resolutions.Add(new Resolution { width = 1280, height = 720 });

        // 드롭다운 옵션
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Count; i++)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height);
        }

        resolutionDropdown.AddOptions(options);

        // 저장값 불러오기
        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        bool isWindowed = PlayerPrefs.GetInt("Windowed", 1) == 1;

        //저장
        selectedIndex = savedIndex;
        selectedwindowed = isWindowed;

        // UI 반영
        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();
        windowedToggle.isOn = isWindowed;

        // 시작
        ApplyResolution(savedIndex, isWindowed);

        // 이벤트 연결
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        windowedToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    public void OnResolutionChanged(int index)
    {
        selectedIndex = index;
    }

    public void OnToggleChanged(bool isWindowed)
    {
        selectedwindowed = isWindowed;
    }

    // Apply 버튼
    public void OnApply()
    {
        ApplyResolution(selectedIndex, selectedwindowed);

        PlayerPrefs.SetInt("ResolutionIndex", selectedIndex);
        PlayerPrefs.SetInt("Windowed", selectedwindowed ? 1 : 0);
    }

    void ApplyResolution(int index, bool isWindowed)
    {
        Resolution res = resolutions[index];

        FullScreenMode mode = isWindowed
            ? FullScreenMode.Windowed
            : FullScreenMode.FullScreenWindow;

        Screen.SetResolution(res.width, res.height, mode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
