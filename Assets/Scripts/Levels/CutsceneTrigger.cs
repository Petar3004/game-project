using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneTrigger : MonoBehaviour
{
    public float secondsBeforeCutscene = 1;
    public int cutsceneIndex;
    public Vector3 playerPos;

    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(LoadCutscene());
    }

    IEnumerator LoadCutscene()
    {
        yield return new WaitForSeconds(secondsBeforeCutscene);

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(cutsceneIndex);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        ManagersRoot.instance.gameManager.gameStarted = false;
    }
}
