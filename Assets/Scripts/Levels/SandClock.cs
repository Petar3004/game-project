using System.Collections;
using TMPro;
using UnityEngine;

public class SandClock : MonoBehaviour
{
    public int sequenceLength;
    private string targetSequence;
    private string currentSequence;
    public GameObject pivot;
    public TMP_Text text;
    private Coroutine spinCoroutine;
    public bool completed;

    void Start()
    {
        GenerateRandomSequence();
    }

    private void GenerateRandomSequence()
    {
        for (int i = 0; i < sequenceLength; i++)
        {
            float randFloat = Random.Range(0f, 1f);
            if (randFloat < 0.5f)
            {
                targetSequence += 'L';
            }
            else
            {
                targetSequence += 'R';
            }
        }
        text.text = targetSequence;
    }

    public void SpinClock(ButtonType buttonType)
    {
        if (spinCoroutine == null && !completed)
        {
            spinCoroutine = StartCoroutine(SpinRoutine(buttonType));
        }

        UpdateSequence(buttonType);
    }

    private IEnumerator SpinRoutine(ButtonType buttonType)
    {
        float angle = 0;
        if (buttonType == ButtonType.LEFT)
        {
            while (angle < 180)
            {
                transform.RotateAround(pivot.transform.position, Vector3.forward, 1);
                angle++;
                yield return null;
            }
        }
        else
        {
            while (angle > -180)
            {
                transform.RotateAround(pivot.transform.position, Vector3.forward, -1);
                angle--;
                yield return null;
            }
        }
        spinCoroutine = null;
    }

    private void UpdateSequence(ButtonType buttonType)
    {
        if (buttonType == ButtonType.LEFT)
        {
            currentSequence += 'L';
        }
        else
        {
            currentSequence += 'R';
        }

        if (currentSequence.Length >= targetSequence.Length && currentSequence.Substring(currentSequence.Length - targetSequence.Length) == targetSequence)
        {
            completed = true;
        }
    }
}
