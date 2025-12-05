using System.Collections;
using UnityEngine;

public class GearMovement : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 3f;
    [Range(0f, 360f)]
    public float angle = 0;
    public float pauseTimeSeconds = 0;
    private Vector3 startPosition;
    private bool currentDirection;
    private bool lastDirection;
    private float minBoundX;
    private float minBoundY;
    private float maxBoundX;
    private float maxBoundY;

    void Start()
    {
        startPosition = transform.position;
        InitializeBounds();
        StartCoroutine(Move());
    }
    void InitializeBounds()
    {
        angle = angle * Mathf.Deg2Rad;
        minBoundX = startPosition.x;
        minBoundY = startPosition.y;
        maxBoundX = startPosition.x + Mathf.Sin(angle) * distance;
        maxBoundY = startPosition.y + Mathf.Cos(angle) * distance;
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
        float targetX = currentDirection ? maxBoundX : minBoundX;
        float targetY = currentDirection ? maxBoundY : minBoundY;
        float currentSpeed = speed;

        if (TimeManager.instance != null && TimeManager.instance.isSlowed)
        {
            currentSpeed *= TimeManager.instance.slowTimeFactor;
        }

        float newX = Mathf.MoveTowards(pos.x, targetX, currentSpeed * Time.deltaTime);
        float newY = Mathf.MoveTowards(pos.y, targetY, currentSpeed * Time.deltaTime);
        pos.x = newX;
        pos.y = newY;
        transform.position = pos;

        if (newX == targetX && newY == targetY)
        {
            currentDirection = !currentDirection;
        }
    }
}