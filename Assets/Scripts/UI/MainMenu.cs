using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject chapters;
    public GameObject complete;
    public TMP_Text continueText;

    void Start()
    {
        UpdateSavedUI();

        if (ManagersRoot.instance.gameManager.chapterComplete)
        {
            ChapterComplete();
        }
    }

    public void NewGame()
    {
        ManagersRoot.instance.gameManager.ResetProgress();
        Continue();
    }

    public void Continue()
    {
        int savedLevel = ManagersRoot.instance.gameManager.savedLevel;
        if (savedLevel != -1)
        {
            ManagersRoot.instance.sceneController.GoToLevel(savedLevel);
        }
        else
        {
            ManagersRoot.instance.sceneController.GoToLevel(1);
        }
        ManagersRoot.instance.gameManager.gameStarted = true;
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

    private void UpdateSavedUI()
    {
        if (continueText == null)
        {
            return;
        }

        int savedLevel = ManagersRoot.instance.gameManager.savedLevel;
        if (savedLevel == -1)
        {
            return;
        }

        int chapter = Mathf.CeilToInt((float)savedLevel / 3);
        int level = savedLevel % 3;
        if (level == 0)
        {
            level = 3;
        }
        continueText.text = "Continue (" + chapter + "/" + level + ")";
    }

    public void ChapterComplete()
    {
        gameObject.SetActive(false);
        complete.SetActive(true);
    }
}
