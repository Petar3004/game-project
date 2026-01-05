using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

    private void Awake()
    {
        if (sprites.Length == 0) return;

        var renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = sprites[Random.Range(0, sprites.Length)];

        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        transform.localScale = new Vector3(
                                        Random.Range(0.5f, 1.5f) * transform.localScale.x,
                                        Random.Range(0.5f, 1.5f) * transform.localScale.y,
                                        transform.localScale.z);
    }
}