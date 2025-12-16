using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private GameObject pauseMenu;
    public bool isPaused = false;

    void Start()
    {
        pauseMenu = GameObject.Find("Pause Menu");
        pauseMenu.SetActive(false);
    }

    public void Pause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
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
