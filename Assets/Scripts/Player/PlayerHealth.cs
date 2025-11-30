using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 1;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            GameManager.instance.RestartLevel();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
