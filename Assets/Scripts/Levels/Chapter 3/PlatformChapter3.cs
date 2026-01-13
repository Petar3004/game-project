using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlatformChapter3 : MonoBehaviour
{
    [Header("Movement Toggle")]
    public bool moving = false;

    [Header("Move Settings")]
    public float moveSpeed = 2f;
    public float distance = 3f;
    public float pauseTimeSeconds = 0f;

    private Rigidbody2D platformRb;
    private Vector2 startPosition;
    private Vector2 pointA;
    private Vector2 pointB;

    private bool movingRight = true;
    private bool waiting = false;

    private void Awake()
    {
        platformRb = GetComponent<Rigidbody2D>();
        platformRb.bodyType = RigidbodyType2D.Kinematic;
        platformRb.gravityScale = 0f;
    }

    private void Start()
    {
        if (!moving)
            return;

        startPosition = platformRb.position;
        InitializeBounds();
    }

    private void FixedUpdate()
    {
        if (!moving || waiting)
            return;

        Vector2 target = movingRight ? pointB : pointA;

        platformRb.MovePosition(
            Vector2.MoveTowards(
                platformRb.position,
                target,
                moveSpeed * Time.fixedDeltaTime
            )
        );

        if (Vector2.Distance(platformRb.position, target) < 0.01f)
        {
            StartCoroutine(SwitchDirection());
        }
    }

    private void InitializeBounds()
    {
        pointA = startPosition;
        pointB = startPosition + Vector2.right * distance;
    }

    private System.Collections.IEnumerator SwitchDirection()
    {
        waiting = true;
        yield return new WaitForSeconds(pauseTimeSeconds);
        movingRight = !movingRight;
        waiting = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (!moving) return;

        Gizmos.color = Color.cyan;
        Vector3 start = Application.isPlaying ? (Vector3)startPosition : transform.position;
        Vector3 end = start + Vector3.right * distance;

        Gizmos.DrawLine(start, end);
        Gizmos.DrawSphere(start, 0.1f);
        Gizmos.DrawSphere(end, 0.1f);
    }
}
