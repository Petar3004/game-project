using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [Header("Timer settings")]
    public float maxTime = 60f;
    public float maxTimePuzzle = 80f;
    public float timeLeft;

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex % 3 == 0 && ManagersRoot.instance.gameManager.gameStarted)
        {
            maxTime = maxTimePuzzle;
        }
        timeLeft = maxTime;
        UIRoot.instance.UpdateMaxTimerUI(maxTime);
    }

    void Update()
    {
        if (!ManagersRoot.instance.gameManager.gameStarted)
        {
            return;
        }

        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0)
            {
                timeLeft = 0;
                Debug.Log("Time's Up!");
                ManagersRoot.instance.gameManager.RestartLevel();
            }
        }
        UIRoot.instance.UpdateTimerUI();
    }

    public void ResetTimer()
    {
        timeLeft = maxTime;
    }
}
