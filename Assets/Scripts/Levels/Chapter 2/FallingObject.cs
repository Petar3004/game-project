using System.Collections;
using UnityEngine;
public enum FallingObjectType
{
    TRASH,
    COLLECTABLE
}

public class FallingObject : MonoBehaviour
{
    public FallingObjectType fallingObjectType;
    public SpriteRenderer spriteRenderer;
    private SandClockPuzzle puzzle;
    public Sprite[] sprites;

    void Start()
    {
        if (fallingObjectType == FallingObjectType.COLLECTABLE)
        {
            spriteRenderer.sprite = sprites[0];
        }
        else
        {
            spriteRenderer.sprite = sprites[Random.Range(1, 3)];
        }

        puzzle = GameObject.Find("Puzzle").GetComponent<SandClockPuzzle>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerObject"))
        {
            if (fallingObjectType == FallingObjectType.COLLECTABLE)
            {
                puzzle.GetSequencePiece();
                Destroy(gameObject);
            }
        }
        else
        {
            StartCoroutine(DestroyObject());
        }
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSecondsRealtime(1);

        Destroy(gameObject);
    }
}