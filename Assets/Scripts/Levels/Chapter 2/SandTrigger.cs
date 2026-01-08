using UnityEngine;

public class SandTrigger : MonoBehaviour
{
    public SandRising sand;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(sand.StartRising());
        }
    }
}
