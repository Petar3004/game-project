using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
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
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelIndex);
        while (!asyncLoad.isDone)
            yield return null;

        ManagersRoot.instance.playerManager.SpawnPlayer(levelIndex, 0);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
