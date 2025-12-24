using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Burst.CompilerServices;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public bool smallHintIsShown = false;
    public bool bigHintIsShown = false;
    private List<string> currentBigHints;
    private int currentHintIndex;
    public Dictionary<string, HintType> unlockedHints = new Dictionary<string, HintType>();
    private Coroutine smallHintRoutine;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (smallHintIsShown)
            {
                HideSmallHint();
            }
            if (bigHintIsShown)
            {
                ShowNextBigHint();
            }
        }
    }

    public void ShowFirstBigHint(List<string> hints)
    {
        currentBigHints = hints;
        currentHintIndex = 0;
        bigHintIsShown = true;
        ManagersRoot.instance.pauseManager.Pause(showPauseScreen: false);
        UIRoot.instance.ShowHintUI(currentBigHints[currentHintIndex], HintType.BIG);
        UnlockHint(currentBigHints[0], HintType.BIG);
    }

    public void ShowNextBigHint()
    {
        if (currentHintIndex < currentBigHints.Count - 1)
        {
            currentHintIndex++;
            UIRoot.instance.ShowHintUI(currentBigHints[currentHintIndex], HintType.BIG);
            UnlockHint(currentBigHints[currentHintIndex], HintType.BIG);
        }
        else
        {
            HideBigHint();
        }
    }

    public void ShowSmallHintForSeconds(string hint, float seconds)
    {
        smallHintRoutine = StartCoroutine(SmallHintRoutine(hint, seconds));
    }

    private IEnumerator SmallHintRoutine(string hint, float seconds)
    {
        smallHintIsShown = true;
        UIRoot.instance.ShowHintUI(hint, HintType.SMALL);
        UnlockHint(hint, HintType.SMALL);
        yield return new WaitForSecondsRealtime(seconds);
        UIRoot.instance.HideHintUI();
        smallHintIsShown = false;
    }

    public void HideBigHint()
    {
        ManagersRoot.instance.pauseManager.Resume();
        bigHintIsShown = false;
        UIRoot.instance.HideHintUI();
    }

    public void HideSmallHint()
    {
        if (!smallHintIsShown)
        {
            return;
        }
        smallHintIsShown = false;
        StopCoroutine(smallHintRoutine);
        UIRoot.instance.HideHintUI();
    }

    public void UnlockHint(string hint, HintType type)
    {
        if (!unlockedHints.ContainsKey(hint))
        {
            unlockedHints.Add(hint, type);
        }
    }
}