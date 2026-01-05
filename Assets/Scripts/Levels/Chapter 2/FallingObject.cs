using System.Collections;
using UnityEngine;
public enum FallingObjectType
{
    ENEMY,
    COLLECTABLE
}

public class FallingObject : MonoBehaviour
{
    public FallingObjectType fallingObjectType;
    public EnemyDamage enemyDamage;
    public SpriteRenderer spriteRenderer;
    private SandClockPuzzle puzzle;
    public Sprite[] sprites;

    void Start()
    {
        if (fallingObjectType == FallingObjectType.COLLECTABLE)
        {
            spriteRenderer.sprite = sprites[0];
            enemyDamage.damage = 0;
        }
        else
        {
            spriteRenderer.sprite = sprites[Random.Range(1, 3)];
            enemyDamage.damage = 1;
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
            if (fallingObjectType == FallingObjectType.ENEMY)
            {
                enemyDamage.damage = 0;
                StartCoroutine(DestroyObject());
            }
        }
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSecondsRealtime(1);

        Destroy(gameObject);
    }
}