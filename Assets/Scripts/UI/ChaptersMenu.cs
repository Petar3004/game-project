using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChaptersMenu : MonoBehaviour
{
    int currentChapter = 1;
    int currentLevel = 1;
    public TMP_Text chapter;
    public TMP_Text level;
    public GameObject main;
    Dictionary<int, int[]> chapterToLevel = new Dictionary<int, int[]>
    {
        { 1, new int[] {1, 2, 3 } },
        { 2, new int[] {4, 5, 6 } },
        { 3, new int[] {7, 8, 9 } }
    };

    public void NextChapter()
    {
        if (currentChapter + 1 <= 3)
        {
            currentChapter++;
            chapter.text = "Chapter " + currentChapter;
        }
    }

    public void PrevChapter()
    {
        if (currentChapter - 1 >= 1)
        {
            currentChapter--;
            chapter.text = "Chapter " + currentChapter;
        }
    }

    public void NextLevel()
    {
        if (currentLevel + 1 <= 3)
        {
            currentLevel++;
            level.text = "Level " + currentLevel;
        }
    }

    public void PrevLevel()
    {
        if (currentLevel - 1 >= 1)
        {
            currentLevel--;
            level.text = "Level " + currentLevel;
        }
    }

    public void PlayLevel()
    {
        ManagersRoot.instance.sceneController.GoToLevel(chapterToLevel[currentChapter][currentLevel - 1]);
        ManagersRoot.instance.gameManager.gameStarted = true;
        UIRoot.instance.SetActiveSpecial();
    }

    public void Back()
    {
        gameObject.SetActive(false);
        main.SetActive(true);
    }
}
