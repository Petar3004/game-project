using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    private int currentHealth;
    public int maxHealth = 1;

    private Animator animator;
    private bool isDead = false;
    private PlayerMovement playerMovement;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.Play("damage");
        }
    }

    private void Die()
    {
        isDead = true;
        currentHealth = 0;

        if (playerMovement != null)
        {
            playerMovement.TriggerDeath();
        }
        else
        {
            animator.Play("death");
        }

        StartCoroutine(WaitAndRestartLevel());
    }

    IEnumerator WaitAndRestartLevel()
    {
        yield return new WaitForSeconds(1f);

        ManagersRoot.instance.gameManager.RestartLevel();
    }


    public void Heal(int amount)
    {
        if (isDead) return;
        currentHealth += amount;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;

        if (playerMovement != null)
        {
            playerMovement.LockPosition(false);
        }
        else if (animator != null)
        {
            animator.Play("idle");
        }
    }
}