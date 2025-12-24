using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChaptersMenu : MonoBehaviour
{
    private int currentChapter = 1;
    private int currentLevel = 1;
    public TMP_Text chapter;
    public TMP_Text level;
    public GameObject main;
    public GameObject[] clocks;
    private GameObject currentClock;
    public Image[] clock1Overlays;
    public Image[] clock2Overlays;
    public Image[] clock3Overlays;
    private Image[] currentOverlays;

    void Start()
    {
        currentClock = clocks[0];
        currentOverlays = clock1Overlays;

        chapter.text = "Chapter " + currentChapter;
        level.text = "Level " + currentLevel;

        ChangeImage();
        ChangeOverlays();
    }

    public void NextChapter()
    {
        if (currentChapter + 1 > 3)
        {
            return;
        }
        currentChapter++;
        chapter.text = "Chapter " + currentChapter;
        currentLevel = 1;
        level.text = "Level " + currentLevel;
        ChangeImage();
        ChangeOverlays();
    }

    public void PrevChapter()
    {
        if (currentChapter - 1 < 1)
        {
            return;
        }
        currentChapter--;
        chapter.text = "Chapter " + currentChapter;
        currentLevel = 1;
        level.text = "Level " + currentLevel;
        ChangeImage();
        ChangeOverlays();
    }

    public void NextLevel()
    {
        if (currentLevel + 1 > 3)
        {
            return;
        }
        currentLevel++;
        level.text = "Level " + currentLevel;
        ChangeOverlays();
    }

    public void PrevLevel()
    {
        if (currentLevel - 1 < 1)
        {
            return;
        }
        currentLevel--;
        level.text = "Level " + currentLevel;
        ChangeOverlays();
    }

    public void PlayLevel()
    {
        ManagersRoot.instance.sceneController.GoToLevel((currentChapter - 1) * 3 + currentLevel);
        ManagersRoot.instance.gameManager.gameStarted = true;
    }

    public void Back()
    {
        gameObject.SetActive(false);
        main.SetActive(true);
    }

    private void ChangeImage()
    {
        currentClock.SetActive(false);
        currentClock = clocks[currentChapter - 1];
        currentClock.SetActive(true);

        foreach (Image overlay in currentOverlays)
        {
            overlay.gameObject.SetActive(false);
        }
        switch (currentChapter)
        {
            case 1:
                currentOverlays = clock1Overlays;
                break;
            case 2:
                currentOverlays = clock2Overlays;
                break;
            case 3:
                currentOverlays = clock3Overlays;
                break;
        }
        foreach (Image overlay in currentOverlays)
        {
            overlay.gameObject.SetActive(true);
        }
    }

    private void ChangeOverlays()
    {
        Color col = currentOverlays[0].color;
        switch (currentLevel)
        {
            case 1:
                col.a = 0f;
                currentOverlays[0].color = col;
                col.a = 0.5f;
                currentOverlays[1].color = col;
                currentOverlays[2].color = col;
                break;
            case 2:
                col.a = 0f;
                currentOverlays[1].color = col;
                col.a = 0.5f;
                currentOverlays[0].color = col;
                currentOverlays[2].color = col;
                break;
            case 3:
                col.a = 0f;
                currentOverlays[2].color = col;
                col.a = 0.5f;
                currentOverlays[0].color = col;
                currentOverlays[1].color = col;
                break;
        }
    }
}
