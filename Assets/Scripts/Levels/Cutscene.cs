using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public float secondsCutsene;
    public int nextLevel;
    private Coroutine confirmRoutine = null;
    private bool cutsceneRunning = false;

    void Start()
    {
        cutsceneRunning = true;
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
                    ManagersRoot.instance.sceneController.GoToLevel(nextLevel);
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
                ManagersRoot.instance.sceneController.GoToLevel(nextLevel);
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
}
