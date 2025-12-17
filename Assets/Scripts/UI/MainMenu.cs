using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    int savedLevel = -1;
    public GameObject chapters;

    void Start()
    {
        // TODO get saved level from storage
    }

    public void Play()
    {
        if (savedLevel != -1)
        {
            ManagersRoot.instance.sceneController.GoToLevel(savedLevel);
        }
        else
        {
            ManagersRoot.instance.sceneController.GoToLevel(1);
        }
        ManagersRoot.instance.gameManager.gameStarted = true;
        UIRoot.instance.SetActiveSpecial();
    }

    public void Chapters()
    {
        gameObject.SetActive(false);
        chapters.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
