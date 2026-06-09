using UnityEngine;
using TMPro;

public class Quality : MonoBehaviour
{
    public TMP_Dropdown screenModeDropdown;

    private int width = 1920;
    private int height = 1080;

    void Start()
    {
        // 초기화 중 의도치 않은 이벤트 발생 방지
        screenModeDropdown.onValueChanged.RemoveAllListeners();

        // 저장된 화면 모드 불러오기 (0: 전체화면, 1: 창모드)
        int savedMode = PlayerPrefs.GetInt("ScreenModeIndex", 0);

        // 인스펙터 옵션 개수를 벗어나지 않도록 제한
        if (savedMode >= screenModeDropdown.options.Count || savedMode < 0)
            savedMode = 0;

        // 불러온 저장값을 드롭다운 UI에 반영
        screenModeDropdown.value = savedMode;
        screenModeDropdown.RefreshShownValue();

        // 게임 시작 시 실제 화면 모드 적용
        ApplyScreenMode(savedMode);

        // UI 세팅이 완전히 끝난 후 사용자의 조작 이벤트 연결
        screenModeDropdown.onValueChanged.AddListener(OnScreenModeChanged);
    }

    public void OnScreenModeChanged(int index)
    {
        // 사용자가 드롭다운을 바꾸면 화면 모드를 적용하고 값을 저장
        ApplyScreenMode(index);

        PlayerPrefs.SetInt("ScreenModeIndex", index);
        PlayerPrefs.Save();
    }

    void ApplyScreenMode(int index)
    {
        // 인스펙터 순서 기준 -> 0번째: 전체화면, 1번째: 창모드
        FullScreenMode mode = (index == 0)
            ? FullScreenMode.FullScreenWindow
            : FullScreenMode.Windowed;

        Screen.SetResolution(width, height, mode);
    }

    // [테스트용] F1 누르면 창모드 800x600으로 강제 변경
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Screen.SetResolution(800, 600, FullScreenMode.Windowed);
        }
    }
}