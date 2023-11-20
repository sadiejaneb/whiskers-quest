using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private Animator animator;
    private PlayerAttackController attackController;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        attackController = GetComponent<PlayerAttackController>();
    }

    public void ApplyDamage(int damage)
    {
        // Check if the player is blocking
        bool isBlocking = animator.GetBool("IsBlocking");

        // Halve the damage if blocking
        int damageTaken = isBlocking ? damage / 2 : damage;

        currentHealth -= damageTaken;
        Debug.Log("Player health: " + currentHealth);

        if (currentHealth > 0)
        {
            animator.SetTrigger("IsDamaged");
            if (attackController != null)
            {
                StartCoroutine(DelayedDamageSound());
            }
        }

        else if (currentHealth <= 0)
        {
            animator.SetTrigger("IsDead"); // Trigger the death animation
            if (attackController != null)
            {
                attackController.PlayDeathSound();
            }
            Die();
        }
    }

    private IEnumerator DelayedDamageSound()
    {
        yield return new WaitForSeconds(0.4f); // Wait
        attackController.PlayDamageSound();
    }

    public void Die()
    {
        Debug.Log("Player died");
    }
}
