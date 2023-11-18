using UnityEngine;
using System.Collections;

public class BatDamage : MonoBehaviour
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
        else
        {
            StartCoroutine(TriggerDamageAnimation());
        }
    }

    private IEnumerator TriggerDamageAnimation()
    {
        animator.SetTrigger("IsDamaged");
        // Wait for the length of the damage animation
        // Assuming the damage animation is approximately 1 second long
        yield return new WaitForSeconds(1.0f);
        animator.ResetTrigger("IsDamaged");
    }

    private void Die()
    {
        animator.SetTrigger("IsDead"); // Trigger the death animation
        GetComponent<Collider>().enabled = false; // Disable the Collider
        //animator.enabled = false; // Disable the Animator
        
    }
}
