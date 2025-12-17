using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerObject"))
        {
            other.gameObject.GetComponentInChildren<PlayerHealth>().TakeDamage(damage);
        }
    }
}
