using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public void OnclickStart()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnclickLoad()
    {
        Debug.Log("게임 설명");
    }

    public void OnclickOption()
    {
        Debug.Log("설정");
    }

    public void OnclickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}