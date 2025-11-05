using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    [Header("Timer settings")]
    public float totalTime = 60f;
    public Text timerText;
    public Slider timerSlider;

    [Header("Slow Time Settings")]
    public float slowTimeFactor = 0.3f;
    public float slowDuration = 5f;
    public float slowPenalty = 10f;

    private float normalTimeScale = 1f;
    private bool isSlowing = false;

    void Awake()
    {
        // Singleton pattern – only one TimeManager should exist
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (timerText == null || timerSlider == null)
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();
            if (canvas != null)
            {
                timerText = canvas.GetComponentInChildren<Text>();
                timerSlider = canvas.GetComponentInChildren<Slider>();
            }
        }
    }

    void Update()
    {
        if (totalTime > 0)
        {
            totalTime -= Time.deltaTime;
            UpdateTimerUI();

            if (totalTime <= 0)
            {
                totalTime = 0;
                Time.timeScale = 1f;
                Debug.Log("Time's Up!");
                // we now have to implement game over logic here.
            }
        }
    }

    public void ActivateSlowTime()
    {
        if (!isSlowing && totalTime > slowPenalty)
        {
            StartCoroutine(SlowTimeRoutine());
        }
    }

    private IEnumerator SlowTimeRoutine()
    {
        isSlowing = true;
        totalTime -= slowPenalty;
        UpdateTimerUI();

        Time.timeScale = slowTimeFactor;
        yield return new WaitForSecondsRealtime(slowDuration);

        Time.timeScale = normalTimeScale;
        isSlowing = false;
    }

    public void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(totalTime).ToString();
        }

        if (timerSlider != null)
        {
            timerSlider.value = totalTime;
        }
    }


}
