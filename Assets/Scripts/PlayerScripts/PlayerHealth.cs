using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void ApplyDamage(int damage)
    {
        // Check if the player is blocking
        bool isBlocking = animator.GetBool("IsBlocking");

        // Halve the damage if blocking
        int damageTaken = isBlocking ? damage / 2 : damage;

        currentHealth -= damageTaken;
        Debug.Log("Player health: " + currentHealth);

        if (currentHealth <= 0)
        {
            animator.SetTrigger("IsDead"); // Trigger the death animation
        }
    }
}