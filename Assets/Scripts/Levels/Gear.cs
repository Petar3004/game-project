using System.Collections;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 3f;
    [Range(0f, 360f)]
    public float angle = 0;
    public float pauseTimeSeconds = 0;
    private Vector3 startPosition;
    private bool currentDirection;
    private bool lastDirection;
    private Vector3 pointA;
    private Vector3 pointB;


    void Start()
    {
        startPosition = transform.position;
        InitializeBounds();
        StartCoroutine(Move());
    }

    void InitializeBounds()
    {
        float angleRad = angle * Mathf.Deg2Rad;

        pointA = startPosition;
        pointB = startPosition + new Vector3(
            Mathf.Cos(angleRad) * distance,
            Mathf.Sin(angleRad) * distance
        );
    }


    private IEnumerator Move()
    {
        while (true)
        {
            if (lastDirection != currentDirection)
            {
                yield return new WaitForSecondsRealtime(pauseTimeSeconds);
                lastDirection = currentDirection;
            }
            MoveInOneDirection();
            yield return null;
        }
    }

    private void MoveInOneDirection()
    {
        Vector3 pos = transform.position;
        Vector3 target = currentDirection ? pointB : pointA;

        float currentSpeed = speed;
        if (TimeManager.instance != null && TimeManager.instance.isSlowed)
            currentSpeed *= TimeManager.instance.slowTimeFactor;

        transform.position = Vector3.MoveTowards(pos, target, currentSpeed * Time.deltaTime);

        if (transform.position == target)
        {
            currentDirection = !currentDirection;
        }
    }

}