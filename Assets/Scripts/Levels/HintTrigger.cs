using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HintTrigger : MonoBehaviour
{
    public List<string> hints = new List<string>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hints.Count > 0)
        {
            ManagersRoot.instance.hintManager.ShowFirstHint(hints);
        }
    }
}
