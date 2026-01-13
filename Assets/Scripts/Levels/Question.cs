using UnityEngine;

public class Question : MonoBehaviour
{
    public void Abide()
    {
        ManagersRoot.instance.sceneController.GoToCutscene(17);
    }

    public void BeFree()
    {
        ManagersRoot.instance.sceneController.GoToCutscene(18);
    }
}
