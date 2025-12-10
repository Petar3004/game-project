using System.Collections;
using System.Runtime.CompilerServices;
using NUnit.Framework.Constraints;
using UnityEngine;

public enum MovementType
{
    STATIC,
    JUMPING,
    FLYING
}

public enum ThermiteType
{
    ENEMY,
    COLLECTABLE
}

public class Thermite : MonoBehaviour
{
    public MovementType movementType;
    public ThermiteType thermiteType;
    public Rigidbody2D thermiteRb;

    [Header("Jump")]
    public Transform groundCheckCollider;
    public LayerMask groundLayer;
    private float groundCheckRadius = 0.2f;
    private Coroutine jumpRoutine;

    [Header("Fly")]
    public float flyDistance = 10;
    public float flySpeed = -1;
    private int flyDirection;
    private Vector3 flyStartPos;
    private Vector3 flyEndPos;

    [Header("Collectable")]
    public ClockPuzzle clock;

    void Start()
    {
        if (movementType == MovementType.FLYING)
        {
            flyStartPos = transform.position;
            flyEndPos = new Vector3(flyStartPos.x + flyDistance, flyStartPos.y, flyStartPos.z);
            if (flySpeed == -1)
            {
                flySpeed = Random.Range(3f, 8f);
            }
        }
    }

    void Update()
    {
        switch (movementType)
        {
            case MovementType.STATIC:
                break;
            case MovementType.JUMPING:
                if (jumpRoutine == null)
                {
                    jumpRoutine = StartCoroutine(MoveJumping());
                }
                break;
            case MovementType.FLYING:
                MoveFlying();
                break;
        }
    }

    IEnumerator MoveJumping()
    {
        while (true)
        {
            yield return new WaitUntil(IsGrounded);
            float waitTime = Random.Range(0.5f, 2f);
            yield return new WaitForSecondsRealtime(waitTime);
            JumpOnce();
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    void JumpOnce()
    {
        int direction = PickRandomDirection();
        float yMult = 1f;
        if (transform.position.y > 7)
        {
            yMult = 0.5f;
        }
        if (IsGrounded() && transform.position.x < -7.5f)
        {
            direction = 1;
        }
        else if (IsGrounded() && transform.position.y > 7.5f)
        {
            direction = -1;
        }

        thermiteRb.linearVelocity = new Vector2(2.6f * direction, 10f * yMult);
    }

    private void MoveFlying()
    {
        thermiteRb.gravityScale = 0;

        Vector3 pos = transform.position;
        Vector3 target = flyDirection == 1 ? flyStartPos : flyEndPos;

        float currentSpeed = flySpeed;
        if (thermiteType == ThermiteType.ENEMY && TimeManager.instance != null && TimeManager.instance.isSlowed)
        {
            currentSpeed = flySpeed * TimeManager.instance.slowTimeFactor;
        }

        transform.position = Vector3.MoveTowards(pos, target, currentSpeed * Time.deltaTime);

        if (transform.position == target)
        {
            flyDirection = flyDirection == 1 ? -1 : 1;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerObject") && thermiteType == ThermiteType.COLLECTABLE)
        {
            clock.GetRiddlePiece();
            Destroy(gameObject);
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckCollider.position, groundCheckRadius, groundLayer);
    }

    private void OnValidate()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        EnemyDamage enemyDamage = GetComponent<EnemyDamage>();

        if (thermiteType == ThermiteType.COLLECTABLE)
        {
            sprite.color = Color.yellow;
            enemyDamage.damage = 0;
        }
        else
        {
            sprite.color = Color.red;
            enemyDamage.damage = 1;
        }
    }

    private int PickRandomDirection()
    {
        int signPicker = Random.Range(0, 2);
        return signPicker == 0 ? -1 : 1;
    }
}
