using UnityEngine;

public class MovingPlatformVertical : MonoBehaviour
{
    // Public variables you can adjust in Unity Inspector
    public float moveDistance = 3f;      // How far up/down it moves
    public float speed = 1.5f;           // How fast it moves (often slower than horizontal)
    public float waitTime = 1f;          // Time to wait at each end
    
    private Vector2 startPosition;
    private Vector2 topPosition;
    private Vector2 bottomPosition;
    private Vector2 currentTarget;
    private bool isMoving = true;
    private bool movingUp = true;
    
    void Start()
    {
        // Get starting position
        startPosition = transform.position;
        
        // Calculate top and bottom positions
        topPosition = startPosition + Vector2.up * moveDistance;
        bottomPosition = startPosition + Vector2.down * moveDistance;
        
        // Start moving upward
        currentTarget = topPosition;
        movingUp = true;
    }
    
    void Update()
    {
        if (isMoving)
        {
            // Calculate movement
            float step = speed * Time.deltaTime;
            
            // Move towards target
            transform.position = Vector2.MoveTowards(transform.position, currentTarget, step);
            
            // Check if reached target
            if (Vector2.Distance(transform.position, currentTarget) < 0.01f)
            {
                StartCoroutine(WaitAndChangeDirection());
            }
        }
    }
    
    System.Collections.IEnumerator WaitAndChangeDirection()
    {
        isMoving = false;
        
        // Wait at the end
        yield return new WaitForSeconds(waitTime);
        
        // Switch direction
        if (movingUp)
        {
            currentTarget = bottomPosition;    // Go down next
            movingUp = false;
        }
        else
        {
            currentTarget = topPosition;       // Go up next
            movingUp = true;
        }
        
        isMoving = true;
    }
    
    // Make player move with platform
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}