using System.Collections;
using UnityEngine;

public class DissapearingPlatform : MonoBehaviour
{
    public float secondsToFadeOpacityBy1 = 0.01f;
    public int secondsToRegenerate = 3;
    private SpriteRenderer sprite;
    private Collider2D col;

    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        col = gameObject.GetComponent<EdgeCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerObject"))
        {
            StartCoroutine(FadeAndDisappear());
        }
    }

    private IEnumerator FadeAndDisappear()
    {
        Color color = sprite.color;

        for (int i = 0; i <= 100; i++)
        {
            color.a = 1f - (i / 100f);
            sprite.color = color;

            yield return new WaitForSecondsRealtime(secondsToFadeOpacityBy1);
        }

        col.enabled = false;

        yield return new WaitForSecondsRealtime(secondsToRegenerate);

        color.a = 1;
        sprite.color = color;
        col.enabled = true;
    }
}
