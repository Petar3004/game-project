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
            SceneManager.LoadScene(savedLevel);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
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
