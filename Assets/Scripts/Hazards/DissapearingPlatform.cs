using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DissapearingPlatform : MonoBehaviour
{
    public GameObject platform;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerObject"))
        {
            StartCoroutine(FadeAndDisappear());
        }
    }

    private IEnumerator FadeAndDisappear()
    {
        SpriteRenderer sprite = platform.GetComponent<SpriteRenderer>();
        Collider2D col = platform.GetComponent<EdgeCollider2D>();
        Color color = sprite.color;

        for (int i = 0; i <= 100; i++)
        {
            color.a = 1f - (i / 100f);
            sprite.color = color;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        col.enabled = false;

        yield return new WaitForSecondsRealtime(3);

        color.a = 1;
        sprite.color = color;
        col.enabled = true;
    }
}
