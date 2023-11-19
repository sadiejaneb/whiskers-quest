using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDamage : MonoBehaviour
{
    private Animator animator;
    public int health = 100;
    private navigation_patrol patrolScript;

    void Start()
    {
        animator = GetComponent<Animator>();
        patrolScript = GetComponent<navigation_patrol>();
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
    bool CanAttack()
    {
        // Check if the IsDamaged animation is playing
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0 for the base layer
        if (stateInfo.IsName("IsDamaged"))
        {
            return false; // Cannot attack if IsDamaged is playing
        }

        // Add other conditions for attacking, if any
        return true; // Can attack if not in IsDamaged state
    }

    private void Die()
    {
        animator.SetTrigger("IsDead"); // Trigger the death animation
        GetComponent<Collider>().enabled = false; // Disable the Collider
        if (patrolScript != null)
        {
            patrolScript.enabled = false; // Disable the patrol script
        }

        // Disable Rigidbody if exists
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Prevent further physics interactions
            rb.velocity = Vector3.zero; // Stop any residual movement
        }

        animator.SetBool("IsMoving", false);
    }
}
