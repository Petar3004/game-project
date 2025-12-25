using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HintTrigger : MonoBehaviour
{
    public List<string> hints = new List<string>();
    public HintType type;
    public float secondsForSmallHint;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (ManagersRoot.instance.hintManager.unlockedHints.ContainsKey(hints[0]))
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            if (type == HintType.SMALL)
            {
                if (!ManagersRoot.instance.hintManager.smallHintIsShown)
                {
                    ManagersRoot.instance.hintManager.ShowSmallHintForSeconds(hints[0], secondsForSmallHint);
                }
            }
            else
            {
                if (hints.Count > 0)
                {
                    ManagersRoot.instance.hintManager.ShowFirstBigHint(hints);
                }
            }
        }
    }
}
