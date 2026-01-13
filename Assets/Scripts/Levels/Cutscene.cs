using System.Collections;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public float secondsCutsene;
    public int nextLevel;
    private Coroutine confirmRoutine = null;

    void Start()
    {
        StartCoroutine(PlayCutscene(secondsCutsene));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Mouse0))
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

    private IEnumerator PlayCutscene(float secondsCutsene)
    {
        yield return new WaitForSecondsRealtime(secondsCutsene);
        ManagersRoot.instance.sceneController.GoToLevel(nextLevel);
    }

    private IEnumerator ConfirmSkip()
    {
        UIRoot.instance.ShowSkipCutsceneText();
        yield return new WaitForSecondsRealtime(3);
        UIRoot.instance.ShowSkipCutsceneText();
        confirmRoutine = null;
    }
}
