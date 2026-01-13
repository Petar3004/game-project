using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 groundCheckSize;
    private Vector2 wallCheckSize;
    public Transform groundCheckCollider;
    public Transform ceilingCheckCollider;
    public Transform wallCheckColliderLeft;
    public Transform wallCheckColliderRight;
    public Transform squishCheckCollider;
    public LayerMask groundLayer;
    public LayerMask springLayer;
    public LayerMask slowGroundLayer;
    public LayerMask wallLayer;
    public MovementState state = MovementState.STANDING;
    public float moveSpeed = 5f;
    public float crouchSpeed = 3f;
    public float jumpForce = 8f;
    public float springMultiplier = 1.5f;
    private Rigidbody2D playerRb;
    public CapsuleCollider2D standingCollider;
    public CapsuleCollider2D crouchingCollider;
    public SpriteRenderer standingSprite;

    private bool isLocked = false;
    private Animator animator;
    private PlayerHealth playerHealth;

    private string currentAnimState;
    private bool isDead = false;

    const string ANIM_IDLE = "idle";
    const string ANIM_RUN = "run";
    const string ANIM_JUMP = "jump";
    const string ANIM_CROUCH = "crouching";

    public float platformVelocityX = 0;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        groundCheckSize = new Vector2(0.8f * standingCollider.bounds.size.x, 0.1f);
        wallCheckSize = new Vector2(0.05f, 0.9f * standingCollider.bounds.size.y);
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (isDead) return;

        if (IsSquished())
        {
            playerHealth.TakeDamage(3);
        }

        float xInput = 0;
        if (!ManagersRoot.instance.pauseManager.isPaused)
        {
            xInput = Input.GetAxis("Horizontal");
        }

        HandleState();
        HandleHorizontalMovement(xInput);
    }

    public void ChangeAnimation(string newAnimation)
    {
        if (currentAnimState == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimState = newAnimation;
    }

    private void HandleState()
    {
        bool jumpPressed = false;
        bool crouchHeld = false;

        if (!ManagersRoot.instance.pauseManager.isPaused)
        {
            jumpPressed = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
            crouchHeld = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        }

        switch (state)
        {
            case MovementState.STANDING:
                UpdateCollider(false);
                if (jumpPressed && IsGrounded() && !IsSlowed())
                {
                    playerRb.linearVelocityY = jumpForce;
                    state = MovementState.JUMPING;
                    ChangeAnimation(ANIM_JUMP);
                }
                else if (jumpPressed && IsOnSpring())
                {
                    playerRb.linearVelocityY = jumpForce * springMultiplier;
                    state = MovementState.JUMPING;
                    ChangeAnimation(ANIM_JUMP);
                }
                else if (crouchHeld && (IsGrounded() || IsSlowed()))
                {
                    state = MovementState.CROUCHING;
                    ChangeAnimation(ANIM_CROUCH);
                }
                break;

            case MovementState.CROUCHING:
                UpdateCollider(true);
                ChangeAnimation(ANIM_CROUCH);

                if (!crouchHeld && !IsStuck())
                {
                    state = MovementState.STANDING;
                }
                break;

            case MovementState.JUMPING:
                ChangeAnimation(ANIM_JUMP);

                if (IsGrounded() || IsOnSpring())
                {
                    state = MovementState.STANDING;
                }
                break;
        }
    }

    private void HandleHorizontalMovement(float xInput)
    {
        float speed = moveSpeed;
        if (state == MovementState.CROUCHING || IsSlowed())
        {
            speed = crouchSpeed;
        }

        if (xInput > 0)
        {
            standingSprite.flipX = false;
        }
        else if (xInput < 0)
        {
            standingSprite.flipX = true;
        }

        if (Mathf.Sign(xInput) == IsOnWall() || isLocked)
        {
            xInput = 0;
        }

        if (state == MovementState.STANDING)
        {
            if (xInput != 0)
            {
                ChangeAnimation(ANIM_RUN);
            }
            else if (!isLocked)
            {
                ChangeAnimation(ANIM_IDLE);
            }
        }

        Vector2 velocity = playerRb.linearVelocity;

        float targetX = xInput * speed + platformVelocityX;
        float deltaX = targetX - playerRb.linearVelocity.x;

        velocity.x += deltaX;
        velocity.x = Mathf.Clamp(velocity.x, platformVelocityX - speed, platformVelocityX + speed);
        playerRb.linearVelocity = velocity;
    }

    private bool IsSquished()
    {
        return Physics2D.OverlapBox(squishCheckCollider.position, groundCheckSize, 0, slowGroundLayer) && Physics2D.OverlapBox(ceilingCheckCollider.position, groundCheckSize, 0, groundLayer);
    }

    private bool IsOnSpring()
    {
        return Physics2D.OverlapBox(groundCheckCollider.position, groundCheckSize, 0, springLayer);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheckCollider.position, groundCheckSize, 0, groundLayer);
    }

    private bool IsSlowed()
    {
        return Physics2D.OverlapBox(groundCheckCollider.position, groundCheckSize, 0, slowGroundLayer) && !(ManagersRoot.instance.abilityManager.abilityIsActive && ManagersRoot.instance.abilityManager.ability == AbilityType.SAND_SPEED);
    }

    private bool IsStuck()
    {
        return Physics2D.OverlapBox(ceilingCheckCollider.position, groundCheckSize, 0, groundLayer);
    }

    private int IsOnWall()
    {
        if ((Physics2D.OverlapBox(wallCheckColliderLeft.position, wallCheckSize, 0, wallLayer)
        || Physics2D.OverlapBox(wallCheckColliderLeft.position, wallCheckSize, 0, groundLayer))
        && !IsGrounded())
        {
            return -1;
        }
        else if ((Physics2D.OverlapBox(wallCheckColliderRight.position, wallCheckSize, 0, wallLayer)
        || Physics2D.OverlapBox(wallCheckColliderRight.position, wallCheckSize, 0, groundLayer))
        && !IsGrounded())
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    private void UpdateCollider(bool crouched)
    {
        standingCollider.enabled = !crouched;
        crouchingCollider.enabled = crouched;
    }

    public void TriggerDeath()
    {
        isDead = true;
        playerRb.linearVelocity = Vector2.zero;
        ChangeAnimation("death");
    }

    public void LockPosition(bool locked)
    {
        isLocked = locked;
        if (locked)
        {
            if (!isDead)
            {
                state = MovementState.STANDING;
                UpdateCollider(false);
                ChangeAnimation(ANIM_IDLE);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(groundCheckCollider.position, groundCheckSize);
        Gizmos.DrawCube(wallCheckColliderLeft.position, wallCheckSize);
        Gizmos.DrawCube(wallCheckColliderRight.position, wallCheckSize);
        Gizmos.DrawCube(ceilingCheckCollider.position, groundCheckSize);

        Gizmos.color = Color.green;
        Gizmos.DrawCube(squishCheckCollider.position, groundCheckSize);
    }
}

public enum MovementState
{
    STANDING,
    CROUCHING,
    JUMPING
}

