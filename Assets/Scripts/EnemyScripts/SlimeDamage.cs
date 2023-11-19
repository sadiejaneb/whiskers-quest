using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeDamage : MonoBehaviour
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

        if (patrolScript != null) 
        {
            patrolScript.PlayDamageSound();
        }
    }

    private void Die()
    {
        animator.SetTrigger("IsDead"); // Trigger the death animation
        GetComponent<Collider>().enabled = false; // Disable the Collider
        if (patrolScript != null)
        {
            // Stop and disable all audio sources
            // Delay stopping and disabling all audio sources
            StartCoroutine(DisableSoundsAfterDelay());
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
    private IEnumerator DisableSoundsAfterDelay()
    {
        // Wait for the length of the damage sound
        // Assuming the damage sound is approximately 1 second long
        yield return new WaitForSeconds(1.0f);

        // Now stop and disable all audio sources
        patrolScript.StopAllSoundsAndDisableAudioSources();
        patrolScript.enabled = false; // Disable the patrol script
        if (patrolScript.GetComponent<NavMeshAgent>() != null)
        {
            patrolScript.GetComponent<NavMeshAgent>().enabled = false; // Disable NavMeshAgent
        }
    }
}
