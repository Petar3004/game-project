using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 groundCheckSize;
    private Vector2 ceilingCheckSize;
    private Vector2 wallCheckSize;
    public Transform groundCheckCollider;
    public Transform ceilingCheckCollider;
    public Transform wallCheckColliderLeft;
    public Transform wallCheckColliderRight;
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
    public Collider2D standingCollider;
    public Collider2D crouchingCollider;
    public SpriteRenderer standingSprite;
    public SpriteRenderer crouchingSprite;
    private bool isLocked = false;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        groundCheckSize = new Vector2(0.9f * standingCollider.bounds.size.x, 0.2f);
        ceilingCheckSize = new Vector2(0.9f * standingCollider.bounds.size.x, 0.2f);
        wallCheckSize = new Vector2(0.05f, 0.9f * standingCollider.bounds.size.y);

        UpdateSprite(false);
    }

    void Update()
    {
        float xInput = 0;
        if (!ManagersRoot.instance.pauseManager.isPaused)
        {
            xInput = Input.GetAxis("Horizontal");
        }

        HandleState();
        HandleHorizontalMovement(xInput);
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
                UpdateSprite(false);
                UpdateCollider(false);
                if (jumpPressed && IsGrounded() && !IsSlowed())
                {
                    playerRb.linearVelocityY = jumpForce;
                    state = MovementState.JUMPING;
                }
                else if (jumpPressed && IsOnSpring())
                {
                    playerRb.linearVelocityY = jumpForce * springMultiplier;
                    state = MovementState.JUMPING;
                }
                else if (crouchHeld && (IsGrounded() || IsSlowed()))
                {
                    state = MovementState.CROUCHING;
                }
                break;
            case MovementState.CROUCHING:
                UpdateSprite(true);
                UpdateCollider(true);
                if (!crouchHeld && !IsStuck())
                {
                    state = MovementState.STANDING;
                }
                break;
            case MovementState.JUMPING:
                UpdateSprite(false);
                if (IsGrounded() || IsOnSpring())
                {
                    state = MovementState.STANDING;
                }
                break;
        }
    }

    bool IsOnSpring()
    {
        return Physics2D.OverlapBox(groundCheckCollider.position, groundCheckSize, 0, springLayer);
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheckCollider.position, groundCheckSize, 0, groundLayer);
    }

    bool IsSlowed()
    {
        return Physics2D.OverlapBox(groundCheckCollider.position, groundCheckSize, 0, slowGroundLayer) && !(ManagersRoot.instance.abilityManager.abilityIsActive && ManagersRoot.instance.abilityManager.ability == AbilityType.SAND_SPEED);
    }

    bool IsStuck()
    {
        return Physics2D.OverlapBox(ceilingCheckCollider.position, ceilingCheckSize, 0, groundLayer);
    }

    int IsOnWall()
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

    void HandleHorizontalMovement(float xInput)
    {
        float speed = moveSpeed;
        if (state == MovementState.CROUCHING || IsSlowed())
        {
            speed = crouchSpeed;
        }
        if (Mathf.Sign(xInput) == IsOnWall() || isLocked)
        {
            xInput = 0;
        }
        playerRb.linearVelocity = new Vector2(xInput * speed, playerRb.linearVelocityY);
    }

    void UpdateSprite(bool crouched)
    {
        standingSprite.enabled = !crouched;
        crouchingSprite.enabled = crouched;
    }

    void UpdateCollider(bool crouched)
    {
        standingCollider.enabled = !crouched;
        crouchingCollider.enabled = crouched;
    }

    public void PositionLock(bool locked)
    {
        isLocked = locked;

        if (locked)
        {
            state = MovementState.STANDING;
            UpdateSprite(false);
            UpdateCollider(false);
        }
    }
}

public enum MovementState
{
    STANDING,
    CROUCHING,
    JUMPING
}



