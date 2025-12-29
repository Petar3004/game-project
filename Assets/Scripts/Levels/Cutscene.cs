using System.Collections;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public float secondsCutsene;
    public int nextLevel;

    void Start()
    {
        StartCoroutine(PlayCutscene(secondsCutsene));
    }

    private IEnumerator PlayCutscene(float secondsCutsene)
    {
        yield return new WaitForSecondsRealtime(secondsCutsene);
        ManagersRoot.instance.sceneController.GoToLevelFromCutscene(nextLevel);
    }
}
