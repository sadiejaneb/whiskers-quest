using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchDamage : MonoBehaviour
{

    public bool isPunching = false; // Flag to track if the player is currently punching
    private float lastHitTime = 0f;
    public float hitCooldown = 1f;
    public Collider punchCollider;
    private PlayerAttackController playerAttackController;

    void Start()
    {
        playerAttackController = FindObjectOfType<PlayerAttackController>(); // Find and assign the PlayerAttackController
        if (punchCollider != null)
            punchCollider.enabled = false;
    }

    // This method will be called by PlayerAttackController when the punch starts
    public void StartPunch()
    {
        isPunching = true;
        if (punchCollider != null)
            punchCollider.enabled = true; // Enable the collider when the punch starts
    }

    public void EndPunch()
    {
        isPunching = false;
        if (punchCollider != null)
            punchCollider.enabled = false; // Disable the collider when the punch ends
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isPunching) return; // If we're not punching, don't do anything

        // Check if enough time has passed since the last hit
        if (Time.time - lastHitTime < hitCooldown)
            return;

        // Process hit
        ProcessHit(collision);

        // Update the last hit time
        lastHitTime = Time.time;

        Debug.Log("Collision Detected with: " + collision.gameObject.name);

    }
    private void ProcessHit(Collision collision)
    {
        if (isPunching)
        {
            int currentPunchDamage = playerAttackController.punchDamage;
            if (collision.gameObject.CompareTag("Bat"))
            {
                Debug.Log("Bat hit");
                BatDamage batDamage = collision.gameObject.GetComponent<BatDamage>();
                if (batDamage != null)
                {
                    batDamage.ApplyDamage(currentPunchDamage);
                }
            }
            else if (collision.gameObject.CompareTag("Slime"))
            {
                Debug.Log("Slime hit");
                SlimeDamage slimeDamage = collision.gameObject.GetComponent<SlimeDamage>();
                if (slimeDamage != null)
                {
                    slimeDamage.ApplyDamage(currentPunchDamage);
                }
            }
            else if (collision.gameObject.CompareTag("Ghost"))
            {
                Debug.Log("Ghost hit");
                GhostDamage ghostDamage = collision.gameObject.GetComponent<GhostDamage>();
                if (ghostDamage != null)
                {
                    ghostDamage.ApplyDamage(currentPunchDamage);
                }
            }
        }
    }
}
