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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (confirmRoutine == null)
            {
                confirmRoutine = StartCoroutine(ConfirmSkip());
            }
            else
            {
                StopAllCoroutines();
                UIRoot.instance.HideCutsceneUI();
                ManagersRoot.instance.sceneController.GoToLevelFromCutscene(nextLevel);
            }
        }
    }

    private IEnumerator PlayCutscene(float secondsCutsene)
    {
        yield return new WaitForSecondsRealtime(secondsCutsene);
        ManagersRoot.instance.sceneController.GoToLevelFromCutscene(nextLevel);
    }

    private IEnumerator ConfirmSkip()
    {
        UIRoot.instance.ShowCutsceneUI();
        yield return new WaitForSecondsRealtime(3);
        UIRoot.instance.HideCutsceneUI();
        confirmRoutine = null;
    }
}
