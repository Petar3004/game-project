using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionEnter2D(Collision2D player)
    {
        if (player.gameObject.CompareTag("PlayerObject"))
        {
            player.gameObject.GetComponentInChildren<PlayerHealth>().TakeDamage(damage);
        }
    }
}
