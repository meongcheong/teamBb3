    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class CutScenesManager : MonoBehaviour
    {
        public AudioManager audioManager;
        public GameObject[] IntroCutScenePrefabs;
        public GameObject[] EndingCutScenePrefabs;

        public float CutSceneInterval = 6f;

        public GameObject BossDwarfObject;
        public GameObject PlayerObject;
        public GameObject AudioManagerObject;

        public AudioSource CutSceneBGMSource;

        public string TitleSceneName = "TitleScene";

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
        SetGameplayObjects(false);

        if (audioManager != null)
        {
            audioManager.SetCutScenePlaying(true);
        }

        isIntroCutScene = true;
        isEndingCutScene = false;

        PlayCutScene(IntroCutScenePrefabs);
    }

    public void PlayEndingCutScene()
    {
        SetGameplayObjects(false);

        if (audioManager != null)
        {
            audioManager.SetCutScenePlaying(true);
        }

        isIntroCutScene = false;
        isEndingCutScene = true;

        PlayCutScene(EndingCutScenePrefabs);
    }

    void PlayCutScene(GameObject[] cutScenePrefabs)
        {
            if (cutScenePrefabs == null || cutScenePrefabs.Length == 0)
            {
                EndCutScene();
                return;
            }

            CancelInvoke(nameof(ShowCurrentCutScene));

            isPlaying = true;
            currentIndex = 0;
            currentCutScenePrefabs = cutScenePrefabs;

            PlayCutSceneBGM();
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

        StopCutSceneBGM();

        if (currentCutSceneObject != null)
        {
            Destroy(currentCutSceneObject);
        }

        currentCutSceneObject = null;
        currentCutScenePrefabs = null;
        currentIndex = 0;
        isPlaying = false;

        if (isIntroCutScene)
        {
            SetGameplayObjects(true);

            if (audioManager != null)
            {
                audioManager.SetCutScenePlaying(false);
                audioManager.PlayBackgroundBGM();
            }

            isIntroCutScene = false;
            isEndingCutScene = false;
            return;
        }

        if (isEndingCutScene)
        {
            isIntroCutScene = false;
            isEndingCutScene = false;

            SceneManager.LoadScene(TitleSceneName);
            return;
        }
    }

    void SetGameplayObjects(bool active)
        {
            if (BossDwarfObject != null)
            {
                BossDwarfObject.SetActive(active);
            }

            if (PlayerObject != null)
            {
                PlayerObject.SetActive(active);
            }
        }

        public void SetAudioManager(bool active)
        {
            if (audioManager == null)
            {
                return;
            }

            if (active == true)
            {
                audioManager.PlayBackgroundBGM();
            }
            else
            {
                audioManager.StopBackgroundBGM();
            }
        }

        void PlayCutSceneBGM()
        {
            if (CutSceneBGMSource != null)
            {
                CutSceneBGMSource.Stop();
                CutSceneBGMSource.Play();
            }
        }

        void StopCutSceneBGM()
        {
            if (CutSceneBGMSource != null)
            {
                CutSceneBGMSource.Stop();
            }
        }
    }