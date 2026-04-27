using UnityEngine;
using UnityEngine.SceneManagement;

public class Option : MonoBehaviour
{
    public GameObject pauseUI;            // ESC 메뉴
    public GameObject settingUI;          // 설정 전체 패널

    public GameObject settingMainUI; // 설정 1
    public GameObject settingSubUI;  // 설정 2

    void Start()
    {
        pauseUI.SetActive(false);
        settingUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingUI != null && settingUI.activeSelf)
            {
                CloseSetting();
                return;
            }

            if (pauseUI.activeSelf)
                Resume();
            else
                Pause();
        }
    }

    // 게임 멈춤
    public void Pause()
    {
        pauseUI.SetActive(true);
        settingUI.SetActive(false);
        Time.timeScale = 0f;
        Debug.Log("PAUSE");
    }

    // 게임 재개
    public void Resume()
    {
        pauseUI.SetActive(false);
        settingUI.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("RESUME");
    }

    // 🔥 1 → 2
    public void OpenSubSetting()
    {
        settingMainUI.SetActive(false);
        settingSubUI.SetActive(true);
    }

    // 🔥 2 → 1 (뒤로가기)
    public void BackToMainSetting()
    {
        settingSubUI.SetActive(false);
        settingMainUI.SetActive(true);
    }



    // 설정창 열기
    public void OpenSetting()
    {
        pauseUI.SetActive(false);
        settingUI.SetActive(true);
        Debug.Log("SETTING OPEN");
    }

    // 설정창 닫기
    public void CloseSetting()
    {
        settingUI.SetActive(false);
        pauseUI.SetActive(true);
        Debug.Log("SETTING CLOSE");
    }

    // 로비 이동
    public void Lobby()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        Debug.Log("LOBBY");
    }


}