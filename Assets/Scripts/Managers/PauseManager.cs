using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public bool isPaused = false;

    public void Pause(bool showPauseScreen)
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (showPauseScreen)
        {
            UIRoot.instance.ShowPauseUI();
        }
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        UIRoot.instance.HidePauseUI();

        ManagersRoot.instance.hintManager.bigHintIsShown = false;
        ManagersRoot.instance.hintManager.smallHintIsShown = false;
        UIRoot.instance.HideHintUI();
    }

    public void MainMenu()
    {
        isPaused = false;
        UIRoot.instance.HidePauseUI();
        Time.timeScale = 1f;
        ManagersRoot.instance.gameManager.gameStarted = false;
        UIRoot.instance.gameObject.SetActive(false);
        ManagersRoot.instance.sceneController.GoToMainMenu();
    }

    public void Quit()
    {
        isPaused = false;
        Time.timeScale = 1f;
        Application.Quit();
    }
}
