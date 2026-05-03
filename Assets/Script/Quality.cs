using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quality : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public Toggle windowedToggle;

    List<Resolution> resolutions = new List<Resolution>();

    int selectedIndex;
    bool selectedWindowed;

    void Start()
    {
        // 해상도 설정
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

        selectedIndex = savedIndex;
        selectedWindowed = isWindowed;

        // UI 반영
        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();
        windowedToggle.isOn = isWindowed;

        // 시작 시 적용
        ApplyResolution(selectedIndex, selectedWindowed);

        // 이벤트 연결
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        windowedToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    public void OnResolutionChanged(int index)
    {
        selectedIndex = index;

        ApplyResolution(selectedIndex, selectedWindowed);
        PlayerPrefs.SetInt("ResolutionIndex", selectedIndex);
    }

    public void OnToggleChanged(bool isWindowed)
    {
        selectedWindowed = isWindowed;

        ApplyResolution(selectedIndex, selectedWindowed);
        PlayerPrefs.SetInt("Windowed", selectedWindowed ? 1 : 0);
    }

    void ApplyResolution(int index, bool isWindowed)
    {
        Resolution res = resolutions[index];

        FullScreenMode mode = isWindowed
            ? FullScreenMode.Windowed
            : FullScreenMode.FullScreenWindow;

        Screen.SetResolution(res.width, res.height, mode);
    }

    // 창모드 확인용 코드 확인후 지워야함!
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Screen.SetResolution(800, 600, FullScreenMode.Windowed);
        }
    }
}