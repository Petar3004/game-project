using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SlowTimeManager : MonoBehaviour
{

    public static SlowTimeManager instance;
    public float slowTimeDuration;
    public float slowFactor = 0.5f;

    private bool slowTime = false;

    private void Awake()
    {
        // Singleton pattern – only one SlowTimeManager should exist
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsSlowed()
    {
        return slowTime;
    }

    public void TriggerSlowTime()
    {
        StartSlowTimeForSeconds(slowTimeDuration);
    }

    public void StartSlowTimeForSeconds(float duration)
    {
        StartCoroutine(SlowTimeRoutine(duration));
    }

    private IEnumerator SlowTimeRoutine(float duration)
    {
        slowTime = true;
        yield return new WaitForSeconds(duration);
        slowTime = false;
    }
}
