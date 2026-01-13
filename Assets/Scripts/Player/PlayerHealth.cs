using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 1;
    public float invincibleTime = 2;
    private Coroutine invincibleRoutine = null;

    private Animator animator;
    private bool isDead = false;
    private PlayerMovement playerMovement;

    void Start()
    {
        currentHealth = maxHealth;
        UIRoot.instance.UpdateHealthUI();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead || invincibleRoutine != null) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (amount > 0)
            {
                invincibleRoutine = StartCoroutine(MakeInvincible());
            }
        }
        UIRoot.instance.UpdateHealthUI();
    }

    private IEnumerator MakeInvincible()
    {
        SpriteRenderer sprite = ManagersRoot.instance.playerManager.Player.GetComponent<SpriteRenderer>();
        Color col = sprite.color;
        Color newCol = new Color(col.r, col.g, col.b, col.a * 0.3f);

        sprite.color = newCol;
        animator.Play("damage");

        yield return new WaitForSeconds(invincibleTime);

        switch (playerMovement.state)
        {
            case MovementState.CROUCHING:
                animator.Play("crouching");
                break;
            default:
                animator.Play("idle");
                break;
        }
        sprite.color = col;
        invincibleRoutine = null;
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

    // public void Heal(int amount)
    // {
    //     if (isDead) return;
    //     currentHealth += amount;
    // }

    // public void ResetHealth()
    // {
    //     currentHealth = maxHealth;
    //     isDead = false;
    //     UIRoot.instance.UpdateHealthUI();

    //     if (playerMovement != null)
    //     {
    //         playerMovement.LockPosition(false);
    //     }
    //     else if (animator != null)
    //     {
    //         animator.Play("idle");
    //     }
    // }
}