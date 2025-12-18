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
    public SpriteRenderer sprite;
    private SandClockPuzzle puzzle;

    void Start()
    {
        if (fallingObjectType == FallingObjectType.COLLECTABLE)
        {
            sprite.color = Color.yellow;
            enemyDamage.damage = 0;
        }
        else
        {
            sprite.color = Color.red;
            enemyDamage.damage = 1;
        }

        puzzle = GameObject.Find("Puzzle").GetComponent<SandClockPuzzle>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerObject") && fallingObjectType == FallingObjectType.COLLECTABLE)
        {
            puzzle.GetSequencePiece();
            Destroy(gameObject);
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