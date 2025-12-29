using UnityEngine;

public class ChapterComplete : MonoBehaviour
{
    public GameObject main;

    public void Continue()
    {
        ManagersRoot.instance.sceneController.GoToLevel(ManagersRoot.instance.gameManager.savedLevel + 1);
        ManagersRoot.instance.gameManager.gameStarted = true;
        ManagersRoot.instance.gameManager.chapterComplete = false;
    }

    public void MainMenu()
    {
        gameObject.SetActive(false);
        main.SetActive(true);
        ManagersRoot.instance.gameManager.chapterComplete = false;
    }
}
