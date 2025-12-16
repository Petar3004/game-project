using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ClockControlGear : MonoBehaviour
{
    public GameObject player;
    private PlayerMovement playerMovement;
    public Transform playerCheckCollider;
    private Vector2 playerCheckSize = new Vector2(0.2f, 0.2f);
    public LayerMask playerLayer;
    public float gearOrientation = 0;
    public GameObject clockHand;
    public int clockFaceDivisions;
    private bool isRotating;
    public GameObject clockface;
    private bool playerLocked = false;
    public int handOrientation = 0;
    private float timeLocked = 0f;

    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        bool playerInPosition = Physics2D.OverlapBox(playerCheckCollider.position, playerCheckSize, 0, playerLayer) && playerMovement.state == MovementState.STANDING;
        if (playerInPosition)
        {
            if (!playerLocked)
            {
                playerMovement.PositionLock(true);
                playerLocked = true;
                timeLocked = Time.time;
            }
            if (Time.time - timeLocked >= 0.5f)
            {
                SpinGear();
            }
        }
        else
        {
            if (playerLocked)
            {
                playerMovement.PositionLock(false);
                playerLocked = false;
            }
        }
    }

    private void SpinGear()
    {
        float xInput = 0;
        if (!ManagersRoot.instance.pauseManager.isPaused)
        {
            xInput = Input.GetAxis("Horizontal");
        }
        gearOrientation += xInput;
        transform.rotation = Quaternion.Euler(0, 0, gearOrientation);
        if (xInput != 0 && !isRotating)
        {
            int clockwise = xInput > 0 ? -1 : 1;
            StartCoroutine(StepClockHand(clockwise));
        }
    }

    private IEnumerator StepClockHand(int clockwise)
    {
        isRotating = true;

        float stepAngle = 360f / clockFaceDivisions * clockwise;
        float duration = 0.25f;
        float elapsed = 0f;

        float rotatedSoFar = 0f;

        yield return new WaitForSecondsRealtime(0.5f);

        while (elapsed < duration)
        {
            float delta = stepAngle / duration * Time.deltaTime;
            rotatedSoFar += delta;

            clockHand.transform.RotateAround(clockface.transform.position, Vector3.forward, delta);

            elapsed += Time.deltaTime;
            yield return null;
        }

        float correction = stepAngle - rotatedSoFar;
        clockHand.transform.RotateAround(clockface.transform.position, Vector3.forward, correction);

        handOrientation = (int)(handOrientation + stepAngle) % 360;
        if (handOrientation < 0) handOrientation += 360;

        isRotating = false;
    }
}

