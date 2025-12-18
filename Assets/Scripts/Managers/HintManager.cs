using System.Collections.Generic;
using NUnit.Framework;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public bool hintIsShown = false;
    private List<string> hints;
    private int currentHintIndex;

    void Update()
    {
        if (hintIsShown && Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextHint();
        }
    }

    public void ShowFirstHint(List<string> currentHints)
    {
        hints = currentHints;
        currentHintIndex = 0;
        ManagersRoot.instance.pauseManager.Pause(showPauseScreen: false);
        hintIsShown = true;
        UIRoot.instance.ShowHintUI(hints[currentHintIndex]);
    }

    public void ShowNextHint()
    {
        if (currentHintIndex < hints.Count - 1)
        {
            UIRoot.instance.ShowHintUI(hints[++currentHintIndex]);
        }
        else
        {
            HideHint();
        }
    }

    public void HideHint()
    {
        ManagersRoot.instance.pauseManager.Resume();
        hintIsShown = false;
        UIRoot.instance.HideHintUI();
    }
}