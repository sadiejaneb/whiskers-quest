using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDamage : MonoBehaviour
{
    private Animator animator;
    public int health = 100; // Example health value

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("IsDead"); // Trigger the death animation
       // GetComponent<Collider>().enabled = false; // Disable the Collider
    }
}
