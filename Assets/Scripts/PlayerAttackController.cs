using System.Collections;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    private Animator animator;
    private bool isPunching = false; // flag to check if the punch animation is playing
    public int punchDamage = 33; // damage to apply when punching

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Your existing logic for mouse clicks
        if (Input.GetMouseButtonDown(0))
            Debug.Log("Pressed left click.");

        if (Input.GetMouseButtonDown(1))
            Debug.Log("Pressed right click.");

        if (Input.GetMouseButtonDown(2))
            Debug.Log("Pressed middle click.");

        // Logic for punching
        if (Input.GetButtonDown("Fire1") && !isPunching) // Check if "Fire1" is pressed and if not already punching
        {
            StartCoroutine(PlayPunchAnimation()); // Start the punch animation coroutine
        }

        // Block when the button is held down
        if (Input.GetButtonDown("Block"))
        {
            animator.SetBool("IsBlocking", true);
        }

        if (Input.GetButtonUp("Block"))
        {
            animator.SetBool("IsBlocking", false);
        }
    }
    public void Hit()
    {
        // Your code here, e.g., applying damage to an enemy
    }
    void OnTriggerEnter(Collider other)
    {
        if (isPunching)
        {
            if (other.CompareTag("Bat"))
            {
                Debug.Log("Bat hit");
                BatDamage batDamage = other.GetComponent<BatDamage>();
                if (batDamage != null)
                {
                    batDamage.ApplyDamage(punchDamage);
                }
            }
            else if (other.CompareTag("Slime"))
            {
                Debug.Log("Slime hit");
                SlimeDamage slimeDamage = other.GetComponent<SlimeDamage>();
                if (slimeDamage != null)
                {
                    slimeDamage.ApplyDamage(punchDamage);
                }
            }
        }
    }


    private IEnumerator PlayPunchAnimation()
    {
        isPunching = true; // Set the flag to true, indicating the animation is playing
        animator.SetTrigger("PunchRight"); // Using the trigger name as set in Animator Controller

        // Wait for the duration of the animation to play
        yield return new WaitForSeconds(1.0f);

        isPunching = false; // Reset the flag so another punch can be triggered
    }
}