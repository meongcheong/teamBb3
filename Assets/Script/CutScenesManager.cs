using UnityEngine;

public class CutScenesManager : MonoBehaviour
{
    public GameObject[] IntroCutScenePrefabs;
    public GameObject[] EndingCutScenePrefabs;

    public float CutSceneInterval = 6f;

    public GameObject BossDwarfObject;
    public GameObject PlayerObject;

    int currentIndex = 0;
    GameObject currentCutSceneObject;
    GameObject[] currentCutScenePrefabs;

    bool isPlaying = false;
    bool isIntroCutScene = false;
    bool isEndingCutScene = false;

    void Start()
    {
        PlayIntroCutScene();
    }

    public void PlayIntroCutScene()
    {
        if (BossDwarfObject != null)
        {
            BossDwarfObject.SetActive(false);
        }

        if (PlayerObject != null)
        {
            PlayerObject.SetActive(false);
        }

        isIntroCutScene = true;
        isEndingCutScene = false;

        PlayCutScene(IntroCutScenePrefabs);
    }

    public void PlayEndingCutScene()
    {
        if (BossDwarfObject != null)
        {
            BossDwarfObject.SetActive(false);
        }

        if (PlayerObject != null)
        {
            PlayerObject.SetActive(false);
        }

        isIntroCutScene = false;
        isEndingCutScene = true;

        PlayCutScene(EndingCutScenePrefabs);
    }

    void PlayCutScene(GameObject[] cutScenePrefabs)
    {
        if (isPlaying == true)
        {
            return;
        }

        if (cutScenePrefabs == null || cutScenePrefabs.Length == 0)
        {
            EndCutScene();
            return;
        }

        CancelInvoke(nameof(ShowCurrentCutScene));

        isPlaying = true;
        currentIndex = 0;
        currentCutScenePrefabs = cutScenePrefabs;

        ShowCurrentCutScene();
    }

    void ShowCurrentCutScene()
    {
        if (currentCutSceneObject != null)
        {
            Destroy(currentCutSceneObject);
        }

        if (currentIndex >= currentCutScenePrefabs.Length)
        {
            EndCutScene();
            return;
        }

        currentCutSceneObject = Instantiate(currentCutScenePrefabs[currentIndex]);

        currentIndex++;

        Invoke(nameof(ShowCurrentCutScene), CutSceneInterval);
    }

    void EndCutScene()
    {
        CancelInvoke(nameof(ShowCurrentCutScene));

        if (currentCutSceneObject != null)
        {
            Destroy(currentCutSceneObject);
        }

        currentCutSceneObject = null;
        currentCutScenePrefabs = null;
        currentIndex = 0;
        isPlaying = false;

        if (isIntroCutScene == true)
        {
            if (BossDwarfObject != null)
            {
                BossDwarfObject.SetActive(true);
            }

            if (PlayerObject != null)
            {
                PlayerObject.SetActive(true);
            }
        }

        if (isEndingCutScene == true)
        {
            Debug.Log("¿£µù ÄÆ¾À Á¾·á");
        }

        isIntroCutScene = false;
        isEndingCutScene = false;
    }
}