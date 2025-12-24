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
        StartCoroutine(LoadLevelAndMovePlayer(currentLevelIndex + 1));
    }

    public void GoToPreviousLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadLevelAndMovePlayer(currentLevelIndex - 1));
    }

    public void GoToLevel(int levelIndex)
    {
        StartCoroutine(LoadLevelAndMovePlayer(levelIndex));
    }

    private IEnumerator LoadLevelAndMovePlayer(int levelIndex)
    {
        ManagersRoot.instance.pauseManager.Pause(false);
        yield return UIRoot.instance.FadeOutCoroutine(sceneFadeDuration);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelIndex);
        while (!asyncLoad.isDone)
            yield return null;

        ManagersRoot.instance.gameManager.ResetLevelParameters();
        ManagersRoot.instance.gameManager.SaveProgress();
        ManagersRoot.instance.playerManager.SpawnPlayer(levelIndex, 0);
        UIRoot.instance.ActivateUI();
        yield return UIRoot.instance.FadeInCoroutine(sceneFadeDuration);
        ManagersRoot.instance.pauseManager.Resume();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
