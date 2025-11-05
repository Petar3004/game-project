using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class VerticalGearMovement : GearMovement
{
    protected override void InitializeBounds()
    {
        if (movingPositive)
        {
            minBound = startPosition.y;
            maxBound = startPosition.y + distance;
        }
        else
        {
            minBound = startPosition.y - distance;
            maxBound = startPosition.y;
        }
    }

    protected override void Move()
    {
        Vector3 pos = transform.position;
        float targetY = movingPositive ? maxBound : minBound;
        float currentSpeed = speed;

        if (TimeManager.instance != null && TimeManager.instance.isSlowed)
        {
            currentSpeed *= TimeManager.instance.slowTimeFactor;
        }

        float newY = Mathf.MoveTowards(pos.y, targetY, currentSpeed * Time.deltaTime);
        pos.y = newY;
        transform.position = pos;

        if (newY == targetY)
        {
            movingPositive = !movingPositive;
        }
    }
}