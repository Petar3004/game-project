using UnityEngine;

public class Room2Gears : MonoBehaviour
{
    [Header("Vertical Motion Settings")]
    public float topY = 3.5f;       // highest point (near ceiling)
    public float bottomY = -3.5f;   // lowest point (near floor)
    public float moveSpeed = 3f;    // movement speed
    public bool startGoingUp = true;

    private bool goingUp;

    void Start()
    {
        // set initial direction
        goingUp = startGoingUp;
    }

    void Update()
    {
        float step = moveSpeed * Time.deltaTime;
        Vector3 pos = transform.position;

        // move either up or down
        if (goingUp)
        {
            pos.y += step;
            if (pos.y >= topY)
            {
                pos.y = topY;
                goingUp = false;   // switch direction
            }
        }
        else
        {
            pos.y -= step;
            if (pos.y <= bottomY)
            {
                pos.y = bottomY;
                goingUp = true;    // switch direction
            }
        }

        transform.position = pos;
    }
}
