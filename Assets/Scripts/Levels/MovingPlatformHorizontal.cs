using UnityEngine;

public class MovingPlatformHorizontal : MonoBehaviour
{
    // Public variables you can adjust in Unity Inspector
    public float moveDistance = 5f;     // How far left/right it moves
    public float speed = 2f;            // How fast it moves
    public float waitTime = 0.5f;       // Time to wait at each end
    
    private Vector2 startPosition;
    private Vector2 leftPosition;
    private Vector2 rightPosition;
    private Vector2 currentTarget;
    private bool isMoving = true;
    private bool movingRight = true;
    
    void Start()
    {
        // Get starting position
        startPosition = transform.position;
        
        // Calculate left and right positions
        leftPosition = startPosition + Vector2.left * moveDistance;
        rightPosition = startPosition + Vector2.right * moveDistance;
        
        // Start moving to the right
        currentTarget = rightPosition;
        movingRight = true;
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
        if (movingRight)
        {
            currentTarget = leftPosition;    // Go left next
            movingRight = false;
        }
        else
        {
            currentTarget = rightPosition;   // Go right next
            movingRight = true;
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