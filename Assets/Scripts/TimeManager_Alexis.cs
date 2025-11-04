using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeManager_Alexis : MonoBehaviour
{
    [Header("Timer Settings")]
    public float totalTime = 60f;          // total time per level
    public Text timerText;                 // drag UI text here
    public float slowTimeScale = 0.3f;     // how slow time becomes
    public float slowDuration = 5f;        // how long the slow lasts (real seconds)
    public float slowPenalty = 10f;        // how many seconds are removed from LevelTime when slowing time

    private bool isSlowing = false;
    private float normalTimeScale = 1f;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        UpdateTimerUI();
    }

    void Update()
    {
        // decrease the timer
        if (totalTime > 0)
        {
            totalTime -= Time.deltaTime;

            // update UI
            UpdateTimerUI();

            // when time runs out, restart
            if (totalTime <= 0)
            {
                totalTime = 0;
                if (gameManager != null)
                    gameManager.RestartGame();
            }
        }
    }

    // called by PlayerController when Left Shift is pressed
    public void ActivateSlowTime()
    {
        // only allow if not already slowing and we have enough time left
        if (!isSlowing && totalTime > slowPenalty)
        {
            StartCoroutine(SlowTimeCoroutine());
        }
    }

    private IEnumerator SlowTimeCoroutine()
    {
        isSlowing = true;

        // apply time penalty
        totalTime -= slowPenalty;
        UpdateTimerUI();

        // slow down time
        Time.timeScale = slowTimeScale;

        // wait 5 real seconds (not affected by timeScale)
        yield return new WaitForSecondsRealtime(slowDuration);

        // return to normal
        Time.timeScale = normalTimeScale;
        isSlowing = false;
    }

    public void ResetTimer()
    {
        totalTime = 60f;
        Time.timeScale = normalTimeScale;
        isSlowing = false;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(totalTime).ToString();
        }
    }
}
