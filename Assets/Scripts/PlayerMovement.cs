using System;
using System.Security.Cryptography;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    const float groundCheckRadius = 0.2f;
    const float ceilingCheckRadius = 0.2f;
    const float wallCheckRadius = 0.05f;
    public Transform groundCheckCollider;
    public Transform ceilingCheckCollider;
    public Transform wallCheckColliderLeft;
    public Transform wallCheckColliderRight;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    static MovementState state = MovementState.STANDING;
    public float moveSpeed = 5f;
    public float crouchSpeed = 3f;
    public float jumpForce = 8f;
    public Rigidbody2D rb;
    public Collider2D standingCollider;
    public SpriteRenderer standingSprite;

    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");

        HandleState();
        HandleHorizontalMovement(xInput);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TimeManager.instance.ActivateSlowTime();
        }
    }

    private void HandleState()
    {
        bool jumpPressed = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        bool crouchHeld = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        switch (state)
        {
            case MovementState.STANDING:
                UpdateSprite(false);
                UpdateCollider(false);
                if (jumpPressed && IsGrounded())
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    state = MovementState.JUMPING;
                }
                else if (crouchHeld && IsGrounded())
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
                if (IsGrounded())
                {
                    state = MovementState.STANDING;
                }
                break;
        }
    }
    
    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckCollider.position, groundCheckRadius, groundLayer);
    }

    bool IsStuck()
    {
        return Physics2D.OverlapCircle(ceilingCheckCollider.position, ceilingCheckRadius, groundLayer);
    }

    int IsOnWall()
    { if (Physics2D.OverlapCircle(wallCheckColliderLeft.position, wallCheckRadius, wallLayer))
        {
            return -1;
        }
        else if (Physics2D.OverlapCircle(wallCheckColliderRight.position, wallCheckRadius, wallLayer))
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
        float speed = (state == MovementState.CROUCHING) ? crouchSpeed : moveSpeed;
        if (Mathf.Sign(xInput) == IsOnWall())
        {
            xInput = 0;
        }
        Debug.Log("Wall " + IsOnWall() + "Input" + xInput);
        rb.linearVelocity = new Vector2(xInput * speed, rb.linearVelocityY);
    }

    void UpdateSprite(bool crouched)
    {
        standingSprite.enabled = !crouched;
    }
    
    void UpdateCollider(bool crouched)
    {
        standingCollider.enabled = !crouched;
    }
}

enum MovementState
{
    STANDING,
    CROUCHING,
    JUMPING
}

