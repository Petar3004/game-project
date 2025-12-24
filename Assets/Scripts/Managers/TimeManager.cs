using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [Header("Timer settings")]
    public float maxTime = 60f;
    public float timeLeft;
    public Text timerText;
    public Slider timerSlider;

    void Start()
    {
        timeLeft = maxTime;
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0)
            {
                timeLeft = 0;
                Debug.Log("Time's Up!");
                ManagersRoot.instance.playerManager.TurnRed();
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
