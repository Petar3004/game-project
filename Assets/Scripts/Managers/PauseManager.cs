using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public bool isPaused = false;

    public void Pause()
    {
        if (ManagersRoot.instance.hintManager.hintIsShown)
        {
            return;
        }
        isPaused = true;
        UIRoot.instance.ShowPauseUI();
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        UIRoot.instance.HidePauseUI();
        Time.timeScale = 1f;
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
