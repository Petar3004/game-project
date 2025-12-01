using System.Collections;
using System.Linq;
using UnityEngine;

public class HorizontalSpike : MonoBehaviour
{
    public GameObject spike;
    public float velX;
    public float velY = 0f;
    Rigidbody2D rb;
    public bool leftToRight;
    private bool playerInRange = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (playerInRange)
        {
            if (leftToRight)
            {
                velX = 5f;
            }
            else
            {
                velX = -5f;
            }
            rb.linearVelocity = new Vector2(velX, velY);
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            StartCoroutine(DestroySpike());
        }
    }

    private IEnumerator DestroySpike()
    {
        yield return new WaitForSecondsRealtime(6);

        Destroy(spike);
    }
}
