using System;
using UnityEngine;

public class InteractiveGear : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D playerRb;
    private PlayerMovement playerMovement;
    public Transform playerCheckCollider;
    private Vector2 playerCheckSize = new Vector2(0.2f, 0.2f);
    public LayerMask playerLayer;
    private float gearOrientation = 0;

    void Start()
    {
        playerRb = player.GetComponent<Rigidbody2D>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Physics2D.OverlapBox(playerCheckCollider.position, playerCheckSize, 0, playerLayer) && playerMovement.state == MovementState.STANDING)
        {
            playerMovement.LockPlayer(true);
            float xInput = Input.GetAxis("Horizontal");
            SpinGear(xInput);
        }
        else
        {
            playerMovement.LockPlayer(false);
        }
    }

    private void SpinGear(float xInput)
    {
        gearOrientation += xInput;
        transform.rotation = Quaternion.Euler(0, 0, gearOrientation);
    }
}

