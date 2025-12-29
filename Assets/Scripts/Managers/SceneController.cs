using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public float sceneFadeDuration = 0.5f;

    public void GoToNextLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadLevelAndMovePlayer(currentLevelIndex + 1, fade: true));
    }

    public void GoToPreviousLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadLevelAndMovePlayer(currentLevelIndex - 1, fade: true));
    }

    public void GoToLevelFromCutscene(int levelIndex)
    {
        StartCoroutine(LoadLevelAndMovePlayer(levelIndex, false));

    }

    public void GoToLevel(int levelIndex)
    {
        StartCoroutine(LoadLevelAndMovePlayer(levelIndex, true));
    }

    private IEnumerator LoadLevelAndMovePlayer(int levelIndex, bool fade)
    {
        ManagersRoot.instance.pauseManager.Pause(false);
        if (fade)
        {
            yield return UIRoot.instance.FadeOutCoroutine(sceneFadeDuration);
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelIndex);
        while (!asyncLoad.isDone)
            yield return null;

        ManagersRoot.instance.gameManager.ResetLevelParameters();
        ManagersRoot.instance.gameManager.unlockedLevels.Add(levelIndex);
        ManagersRoot.instance.gameManager.SaveProgress();
        ManagersRoot.instance.playerManager.SpawnPlayer(levelIndex, 0);
        UIRoot.instance.ActivateUI();
        if (fade)
        {
            yield return UIRoot.instance.FadeInCoroutine(sceneFadeDuration);
        }
        ManagersRoot.instance.pauseManager.Resume();
    }

    public void GoToMainMenu()
    {
        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadMainMenu()
    {
        yield return UIRoot.instance.FadeOutCoroutine(sceneFadeDuration);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
        while (!asyncLoad.isDone)
            yield return null;

        ManagersRoot.instance.gameManager.SaveProgress();
        UIRoot.instance.ActivateUI();
        yield return UIRoot.instance.FadeInCoroutine(sceneFadeDuration);
    }
}
