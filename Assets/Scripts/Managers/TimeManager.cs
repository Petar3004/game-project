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
    public float maxTime = 60f;
    public float timeLeft;
    public Text timerText;
    public Slider timerSlider;

    [Header("Slow Time Settings")]
    public float slowTimeDuration = 5f;
    public float slowTimePenalty = 10f;
    public float slowTimeFactor = 0.5f;
    public bool isSlowed = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

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

    void Start()
    {
        timeLeft = maxTime;
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimerUI();

            if (timeLeft <= 0)
            {
                timeLeft = 0;
                Debug.Log("Time's Up!");
                GameManager.instance.RestartLevel();
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
        UpdateTimerUI();

        yield return new WaitForSecondsRealtime(slowTimeDuration);

        isSlowed = false;
    }

    public void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(timeLeft).ToString();
        }

        if (timerSlider != null)
        {
            timerSlider.value = timeLeft;
        }
    }

    public void ResetTimer()
    {
        timeLeft = maxTime;
    }
}
