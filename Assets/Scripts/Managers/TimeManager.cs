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

    [Header("Slow Time Settings")]
    public float slowTimeDuration = 5f;
    public float slowTimePenalty = 10f;
    public float slowTimeFactor = 0.5f;
    public bool isSlowed = false;

    void Start()
    {
        timeLeft = maxTime;
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UIRoot.instance.UpdateTimerUI(timeLeft);

            if (timeLeft <= 0)
            {
                timeLeft = 0;
                Debug.Log("Time's Up!");
                ManagersRoot.instance.gameManager.RestartLevel();
            }
        }
    }

    public void ActivateSlowTime()
    {
        if (!isSlowed && timeLeft > slowTimePenalty)
        {
            StartCoroutine(StartSlowTime());
        }
    }

    private IEnumerator StartSlowTime()
    {
        isSlowed = true;
        timeLeft -= slowTimePenalty;
        UIRoot.instance.UpdateTimerUI(timeLeft);

        yield return new WaitForSecondsRealtime(slowTimeDuration);

        isSlowed = false;
    }

    public void ResetTimer()
    {
        timeLeft = maxTime;
    }
}
