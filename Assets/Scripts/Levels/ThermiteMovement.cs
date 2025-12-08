using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum MovementType
{
    STATIC,
    JUMPING,
    FLYING
}

public class ThermiteMovement : MonoBehaviour
{
    public MovementType movementType;
    public Rigidbody2D thermiteRb;
    public GameObject player;
    private Coroutine jumpRoutine;
    public Transform groundCheckCollider;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private int flyDirection = 1;

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
            yield return new WaitForSeconds(waitTime);
            JumpOnce();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void JumpOnce()
    {
        int signPicker = Random.Range(0, 2);
        float direction = (signPicker == 0) ? -1.0f : 1.0f;
        float yMult = 1f;
        if (transform.position.y > 7)
        {
            yMult = 0.5f;
        }
        Vector2 jump = new Vector2(2.6f * direction, 10f * yMult);
        thermiteRb.linearVelocity = jump;
    }

    void MoveFlying()
    {
        thermiteRb.gravityScale = 0;

        if (transform.position.x < -7.5f || transform.position.x > 7.5f)
        {
            flyDirection = -flyDirection;
        }
        thermiteRb.linearVelocity = new Vector2(5f * flyDirection, 0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerObject"))
        {
            Destroy(gameObject);
            // TODO: give player riddle fragment
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckCollider.position, groundCheckRadius, groundLayer);
    }
}
