using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class HorizontalGearMovement : GearMovement
{
    protected override void InitializeBounds()
    {
        if (movingPositive)
        {
            minBound = startPosition.x;
            maxBound = startPosition.x + distance;
        }
        else
        {
            minBound = startPosition.x - distance;
            maxBound = startPosition.x;
        }
    }

    protected override void Move()
    {
        Vector3 pos = transform.position;
        float targetX = movingPositive ? maxBound : minBound;
        float currentSpeed = speed;

        if (TimeManager.instance != null && TimeManager.instance.isSlowed)
        {
            currentSpeed *= TimeManager.instance.slowTimeFactor;
        }

        float newX = Mathf.MoveTowards(pos.x, targetX, currentSpeed * Time.deltaTime);
        pos.x = newX;
        transform.position = pos;

        if (newX == targetX)
        {
            movingPositive = !movingPositive;
        }
    }
}