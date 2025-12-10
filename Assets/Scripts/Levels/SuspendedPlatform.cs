using System.Numerics;
using UnityEditor.Callbacks;
using UnityEngine;

public class SuspendedPlatform : MonoBehaviour
{
    public float yOffset = 5f;
    public float speed = 2f;
    private float initY;
    private float targetY;
    private bool isActive = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initY = transform.position.y;
        targetY = initY - yOffset;
    }

    void FixedUpdate()
    {
        float currentSpeed = speed;

        if (TimeManager.instance != null && TimeManager.instance.isSlowed)
        {
            currentSpeed *= TimeManager.instance.slowTimeFactor;
        }

        float target = isActive ? targetY : initY;
        float newY = Mathf.MoveTowards(transform.position.y, target, currentSpeed * Time.deltaTime);
        rb.MovePosition(new UnityEngine.Vector2(rb.position.x, newY));

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isActive = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isActive = false;
        }
    }
}
