using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public float secondsCutsene;
    public int nextLevel;
    private Coroutine confirmRoutine = null;
    private bool cutsceneRunning = false;
    public bool gameOver = false;

    void Start()
    {
        cutsceneRunning = true;
        ManagersRoot.instance.audioManager.StopMusic();
        StartCoroutine(PlayCutscene(secondsCutsene));
    }

    void Update()
    {
        if (cutsceneRunning)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (confirmRoutine == null)
                {
                    confirmRoutine = StartCoroutine(ConfirmSkip());
                }
                else
                {
                    StopAllCoroutines();
                    UIRoot.instance.HideSkipCutsceneText();
                    GoToNextLevel();
                }
            }
        }
        else
        {
            UIRoot.instance.HideSkipCutsceneText();
            UIRoot.instance.ShowContinueText();
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                UIRoot.instance.HideContinueText();
                GoToNextLevel();
                ManagersRoot.instance.pauseManager.Resume();
            }
        }
    }

    private IEnumerator PlayCutscene(float secondsCutsene)
    {
        yield return new WaitForSecondsRealtime(secondsCutsene);
        cutsceneRunning = false;
        ManagersRoot.instance.pauseManager.Pause(showPauseScreen: false);
    }

    private IEnumerator ConfirmSkip()
    {
        UIRoot.instance.ShowSkipCutsceneText();
        yield return new WaitForSecondsRealtime(3);
        UIRoot.instance.HideSkipCutsceneText();
        confirmRoutine = null;
    }

    private void GoToNextLevel()
    {
        if (nextLevel == 0)
        {
            if (gameOver)
            {
                ManagersRoot.instance.gameManager.gameOver = true;
            }
            ManagersRoot.instance.sceneController.GoToMainMenu();
        }
        else if (nextLevel == 10)
        {
            ManagersRoot.instance.sceneController.GoToCutscene(nextLevel);
        }
        else
        {
            ManagersRoot.instance.sceneController.GoToLevel(nextLevel);
        }
    }
}
