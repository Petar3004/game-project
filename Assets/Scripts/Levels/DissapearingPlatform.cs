using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class DissapearingPlatform : MonoBehaviour
{
    public float secondsToFadeOpacityBy1 = 0.01f;
    public int secondsToRegenerate = 3;
    private SpriteRenderer sprite;
    private Collider2D col;
    public bool isStatic = true;
    public bool disappearing = true;
    [Header("Move")]
    public float moveSpeed = 2f;
    public float distance = 3f;
    public float pauseTimeSeconds = 0;
    private Vector3 startPosition;
    private bool currentDirection;
    private bool lastDirection;
    private Vector3 pointA;
    private Vector3 pointB;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        if (!isStatic)
        {
            startPosition = transform.position;
            InitializeBounds();
            currentDirection = true;
            lastDirection = false;
            StartCoroutine(Move());
        }
    }
    void InitializeBounds()
    {
        pointA = startPosition;
        pointB = new Vector3(startPosition.x + distance, startPosition.y, startPosition.z);
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
        Vector3 currentPos = transform.position;
        Vector3 target = currentDirection ? pointB : pointA;

        target.y = currentPos.y;
        target.z = currentPos.z;

        float currentSpeed = moveSpeed;

        transform.position = Vector3.MoveTowards(currentPos, target, currentSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            currentDirection = !currentDirection;
        }
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerObject") && disappearing)
        {
            StartCoroutine(FadeAndDisappear());
        }
    }

    private IEnumerator FadeAndDisappear()
    {
        Color color = sprite.color;

        for (int i = 0; i <= 100; i++)
        {
            color.a = 1f - (i / 100f);
            sprite.color = color;

            yield return new WaitForSecondsRealtime(secondsToFadeOpacityBy1);
        }

        col.enabled = false;

        yield return new WaitForSecondsRealtime(secondsToRegenerate);

        color.a = 1;
        sprite.color = color;
        col.enabled = true;
    }



}
