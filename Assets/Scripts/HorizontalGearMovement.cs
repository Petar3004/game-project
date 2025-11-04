using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class HorizontalGearMovement : MonoBehaviour
{
    public float speed = 2f;
    public float distance;
    public enum GearType
    {
        Central,
        Right
    }

    public GearType gearType;

    private Vector3 startPosition;
    private bool movingRight;
    private float leftBoundX;
    private float rightBoundX;

    void Start()
    {
        startPosition = transform.position;
        if (gearType == GearType.Central)
        {
            leftBoundX = startPosition.x;
            rightBoundX = startPosition.x + distance;
            movingRight = true;
        }
        else
        {
            leftBoundX = startPosition.x - distance;
            rightBoundX = startPosition.x;
            movingRight = false;
        }
    }

    void Update()
    {

        Vector3 pos = transform.position;
        float targetX = movingRight ? rightBoundX : leftBoundX;
        float currentSpeed = speed;

        if (SlowTimeManager.instance != null && SlowTimeManager.instance.IsSlowed())
        {
            currentSpeed *= SlowTimeManager.instance.slowFactor; 
        }

        float newX = Mathf.MoveTowards(pos.x, targetX, currentSpeed * Time.deltaTime);
        pos.x = newX;
        transform.position = pos;

        if (newX == targetX)
        {
            movingRight = !movingRight;
        }
    }

}