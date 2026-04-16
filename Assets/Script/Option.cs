using UnityEngine;
using UnityEngine.SceneManagement;

public class Option : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject settingUI;

    void Start()
    {
        if (pauseUI == null || settingUI == null)
        {
            Debug.LogError("UI 연결 안됨! Inspector 확인해라");
            return;
        }

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

    // 게임 재시작
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("RESTART");
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