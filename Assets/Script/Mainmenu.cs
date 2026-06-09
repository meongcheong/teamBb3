using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void OnclickStart()
    {
        SceneManager.LoadScene("Game");
        audioManager.PlaySFX(audioManager.background);
    }

    public void OnclickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        audioManager.PlaySFX(audioManager.UI);
#else
        Application.Quit();
#endif
    }
}