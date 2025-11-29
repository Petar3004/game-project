using System.Collections;
using System.Linq;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    public GameObject spike;
    public GameObject player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spike.GetComponent<Rigidbody2D>().gravityScale = 1;
            StartCoroutine(DestroySpike());
        }
    }

    private IEnumerator DestroySpike()
    {
        yield return new WaitForSecondsRealtime(3);

        Destroy(spike);
    }
}
