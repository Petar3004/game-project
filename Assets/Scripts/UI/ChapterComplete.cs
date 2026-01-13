using UnityEngine;

public class ChapterComplete : MonoBehaviour
{
    public GameObject main;

    public void Continue()
    {
        int savedLevel = ManagersRoot.instance.gameManager.savedLevel;
        switch (savedLevel)
        {
            case 3:
                ManagersRoot.instance.sceneController.GoToCutscene(14);
                break;
            case 6:
                ManagersRoot.instance.sceneController.GoToCutscene(15);
                break;
            case 9:
                ManagersRoot.instance.sceneController.GoToCutscene(16);
                break;
            default:
                Debug.Log("Saved level: " + savedLevel);
                ManagersRoot.instance.sceneController.GoToLevel(savedLevel + 1);
                break;
        }
        ManagersRoot.instance.gameManager.chapterComplete = false;
    }

    public void MainMenu()
    {
        gameObject.SetActive(false);
        main.SetActive(true);
        ManagersRoot.instance.gameManager.chapterComplete = false;
    }
}
