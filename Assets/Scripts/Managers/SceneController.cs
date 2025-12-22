using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    private Image sceneFadeImage;
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
        yield return UIRoot.instance.FadeOutCoroutine(sceneFadeDuration);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelIndex);
        while (!asyncLoad.isDone)
            yield return null;

        ManagersRoot.instance.playerManager.SpawnPlayer(levelIndex, 0);
        yield return UIRoot.instance.FadeInCoroutine(sceneFadeDuration);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
