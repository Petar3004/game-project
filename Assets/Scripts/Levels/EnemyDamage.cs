using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerObject"))
        {
            ManagersRoot.instance.audioManager.PlaySFX(ManagersRoot.instance.audioManager.death);
            other.gameObject.GetComponentInChildren<PlayerHealth>().TakeDamage(damage);
        }
    }
}
