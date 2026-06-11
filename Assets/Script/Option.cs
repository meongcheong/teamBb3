using UnityEngine;
using UnityEngine.SceneManagement;

public class Option : MonoBehaviour
{
    AudioManager audioManager;

    public GameObject pauseUI;            // ESC 메뉴
    public GameObject settingUI;          // 설정 전체 패널

    public GameObject settingMainUI; // 설정 1
    public GameObject settingSubUI;  // 설정 2

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

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
        audioManager.PlaySFX(audioManager.UI);
        Time.timeScale = 1f;
        Debug.Log("RESUME");
    }

    // 설정 1 → 2
    public void OpenSubSetting()
    {
        settingMainUI.SetActive(false);
        settingSubUI.SetActive(true);
        audioManager.PlaySFX(audioManager.UI);
    }

    // 설정 2 → 1 (뒤로가기)
    public void BackToMainSetting()
    {
        settingSubUI.SetActive(false);
        settingMainUI.SetActive(true);
        audioManager.PlaySFX(audioManager.UI);
    }

    // 설정창 열기
    public void OpenSetting()
    {
        pauseUI.SetActive(false);
        settingUI.SetActive(true);
        Debug.Log("SETTING OPEN");
        audioManager.PlaySFX(audioManager.UI);
    }

    // 설정창 닫기
    public void CloseSetting()
    {
        settingUI.SetActive(false);
        pauseUI.SetActive(true);
        Debug.Log("SETTING CLOSE");
        audioManager.PlaySFX(audioManager.UI);
    }

    // 로비 이동
    public void Lobby()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        Debug.Log("LOBBY");
        audioManager.PlaySFX(audioManager.UI);
    }

    // 게임 재시작
    public void RestartGame()
    {
        Debug.Log("RESTART GAME");
        SceneManager.LoadScene(0);
        audioManager.PlaySFX(audioManager.UI);
    }

    //게임 시작
    public void OnclickStart()
    {
        SceneManager.LoadScene("Game");
        audioManager.PlaySFX(audioManager.background);
    }

    //게임 종료
    public void OnclickQuit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        audioManager.PlaySFX(audioManager.UI);

        Application.Quit();
    }

}