using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_Alexis : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject deathPanel;

    void Start()
    {
        ShowStartScreen();
    }

    public void ShowStartScreen()
    {
        Time.timeScale = 0f; // pause the game
        startPanel.SetActive(true);
        deathPanel.SetActive(false);
    }

    public void StartGame()
    {
        Time.timeScale = 1f; // unpause
        startPanel.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        Time.timeScale = 0f; // freeze the game
        deathPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
