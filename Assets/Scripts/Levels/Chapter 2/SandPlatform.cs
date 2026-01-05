using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class SandPlatform : MonoBehaviour
{
    public float secondsToFadeOpacityBy1 = 0.01f;
    public int secondsToRegenerate = 3;
    private SpriteRenderer sprite;
    public Sprite DisappearingSprite;
    public Sprite nonDisappearingSprite;
    private Collider2D col;
    public bool moving = true;
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
    public Rigidbody2D platformRb;
    private PlayerMovement player;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        if (moving)
        {
            startPosition = platformRb.position;
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

            if (player != null)
            {
                player.platformVelocityX = currentDirection ? moveSpeed : -moveSpeed;
            }

            yield return null;
        }
    }

    private void MoveInOneDirection()
    {
        Vector3 currentPos = platformRb.position;
        Vector3 target = currentDirection ? pointB : pointA;

        target.y = currentPos.y;
        target.z = currentPos.z;

        float currentSpeed = moveSpeed;

        platformRb.MovePosition(
            Vector3.MoveTowards(
                platformRb.position,
                target,
                currentSpeed * Time.fixedDeltaTime
            )
        );

        if (Vector3.Distance(platformRb.position, target) < 0.01f)
        {
            currentDirection = !currentDirection;
        }
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerObject"))
        {
            if (disappearing)
            {
                StartCoroutine(FadeAndDisappear());
            }
            if (moving)
            {
                player = other.gameObject.GetComponent<PlayerMovement>();
                player.platformVelocityX = currentDirection ? moveSpeed : -moveSpeed;
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerObject"))
        {
            if (moving)
            {
                player.platformVelocityX = 0;
                player = null;
            }
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

    private void OnValidate()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();

        if (!disappearing)
        {
            sprite.sprite = nonDisappearingSprite;
            col.offset = new Vector2(col.offset.x, 0);
        }
        else
        {
            sprite.sprite = DisappearingSprite;
            col.offset = new Vector2(col.offset.x, 0.05f);
        }
    }
}
