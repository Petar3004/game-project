using System.Collections;
using System.Linq;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            StartCoroutine(DestroySpike());
        }
    }

    private IEnumerator DestroySpike()
    {
        yield return new WaitForSecondsRealtime(3);

        Destroy(gameObject);
    }
}
