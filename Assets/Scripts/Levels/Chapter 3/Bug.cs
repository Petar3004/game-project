using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BugType
{
    ENEMY,
    COLLECTABLE
}

public class Bug : MonoBehaviour
{
    public BugType type;
    public SpriteRenderer sprite;
    public Sprite[] sprites;
    public EnemyDamage enemyDamage;

    [Header("Enemy")]
    public float speed = 2f;
    public GameObject[] cables;
    public float nodeEpsilon = 0.3f;
    public float teleportDelay = 1f;
    public float fadeDelay = 3f;

    [Header("Collectable")]
    public DigitalClock clock;

    private List<CableSegment> allSegments = new List<CableSegment>();
    private CableSegment currentSeg;
    private CableSegment previousSeg;
    private Vector2 currentTarget;
    private bool initialized;
    private bool isWaiting; // Prevents movement during timeout
    private Coroutine fadeRoutine;

    public class CableSegment
    {
        public Vector2 p1, p2;
        public List<CableSegment> connections = new List<CableSegment>();
    }

    void Start()
    {
        if (type == BugType.ENEMY)
        {
            foreach (GameObject cable in cables)
            {
                EdgeCollider2D ec = cable.GetComponent<EdgeCollider2D>();
                for (int i = 0; i < ec.pointCount - 1; i++)
                {
                    allSegments.Add(new CableSegment
                    {
                        p1 = cable.transform.TransformPoint(ec.points[i]),
                        p2 = cable.transform.TransformPoint(ec.points[i + 1])
                    });
                }
            }

            foreach (CableSegment s1 in allSegments)
            {
                foreach (CableSegment s2 in allSegments)
                {
                    if (s1 == s2) continue;
                    if (Vector2.Distance(s1.p1, s2.p1) < nodeEpsilon || Vector2.Distance(s1.p1, s2.p2) < nodeEpsilon ||
                        Vector2.Distance(s1.p2, s2.p1) < nodeEpsilon || Vector2.Distance(s1.p2, s2.p2) < nodeEpsilon)
                    {
                        s1.connections.Add(s2);
                    }
                }
            }

            TeleportToRandomSegment();
            initialized = true;
        }
        else
        {
            clock = GameObject.Find("Clock").GetComponent<DigitalClock>();
        }
    }

    void Update()
    {
        if (type == BugType.ENEMY)
        {
            if (!initialized || isWaiting) return;

            transform.position = Vector2.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, currentTarget) < 0.001f)
            {
                SwitchToNextSegment();
            }

            if (ManagersRoot.instance.abilityManager.abilityIsActive && fadeRoutine != null)
            {
                StopCoroutine(fadeRoutine);
                enemyDamage.damage = 0;
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
                GetComponent<CircleCollider2D>().radius = 0;
            }
            else
            {
                enemyDamage.damage = 1;
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
                GetComponent<CircleCollider2D>().radius = 0.5f;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerObject") && type == BugType.COLLECTABLE)
        {
            clock.GetRiddlePiece();
            Destroy(gameObject);
        }
    }

    void SwitchToNextSegment()
    {
        Vector2 reachedNode = currentTarget;
        List<(CableSegment segment, Vector2 nextPoint)> options = new List<(CableSegment, Vector2)>();

        foreach (CableSegment neighbor in currentSeg.connections)
        {
            if (neighbor == previousSeg) continue;

            if (Vector2.Distance(neighbor.p1, reachedNode) < nodeEpsilon)
                options.Add((neighbor, neighbor.p2));
            else if (Vector2.Distance(neighbor.p2, reachedNode) < nodeEpsilon)
                options.Add((neighbor, neighbor.p1));
        }

        if (options.Count > 0)
        {
            (CableSegment segment, Vector2 nextPoint) choice = options[Random.Range(0, options.Count)];
            previousSeg = currentSeg;
            currentSeg = choice.segment;
            currentTarget = choice.nextPoint;
        }
        else
        {
            StartCoroutine(WaitAndTeleport());
        }
    }

    private IEnumerator FadeSprite(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = sprite.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            sprite.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }

        sprite.color = new Color(color.r, color.g, color.b, endAlpha);
        enemyDamage.damage = 1;
    }

    private IEnumerator WaitAndTeleport()
    {
        isWaiting = true;
        enemyDamage.damage = 0;

        yield return new WaitForSeconds(teleportDelay);

        TeleportToRandomSegment();

        yield return fadeRoutine = StartCoroutine(FadeSprite(0f, 1f, fadeDelay));

        isWaiting = false;
    }

    private void TeleportToRandomSegment()
    {
        if (allSegments.Count == 0) return;

        currentSeg = allSegments[Random.Range(0, allSegments.Count)];
        previousSeg = null;

        if (Random.value > 0.5f)
        {
            transform.position = currentSeg.p1;
            currentTarget = currentSeg.p2;
        }
        else
        {
            transform.position = currentSeg.p2;
            currentTarget = currentSeg.p1;
        }
    }

    void OnValidate()
    {
        if (type == BugType.ENEMY)
        {
            enemyDamage.damage = 1;
            sprite.sprite = sprites[0];
            sprite.color = Color.skyBlue;
        }
        else
        {
            enemyDamage.damage = 0;
            sprite.sprite = sprites[1];
            sprite.color = Color.yellow;
        }
    }
}