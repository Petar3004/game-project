using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public bool hintIsShown = false;

    // (level, position), hint
    Dictionary<(int, Vector3), string> hints = new Dictionary<(int, Vector3), string>
    {
        {(1, new Vector3(-24.98f, -3.08f, 0)), "hint 1"}
    };

    void Update()
    {
        if (hintIsShown && Input.GetKeyDown(KeyCode.Space))
        {
            HideHint();
        }
    }

    public void ShowHint(int level, Vector3 pos)
    {
        Time.timeScale = 0;
        hintIsShown = true;
        UIRoot.instance.ShowHintUI(hints[(level, pos)]);
    }

    public void HideHint()
    {
        Time.timeScale = 1;
        hintIsShown = false;
        UIRoot.instance.HideHintUI();
    }
}