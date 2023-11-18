using System.Collections;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    private Animator animator;
    private bool isPunching = false; // flag to check if the punch animation is playing
    public int punchDamage = 33; // damage to apply when punching
    public GameObject punchCollider; // Assign this in the Inspector to the GameObject with the PunchDamage script

    private PunchDamage punchDamageScript; // Reference to the PunchDamage script

    void Start()
    {
        animator = GetComponent<Animator>();
        punchDamageScript = punchCollider.GetComponent<PunchDamage>(); // Get the PunchDamage script
    }

    void Update()
    {
        // Logic for punching
        if (Input.GetButtonDown("Fire1") && !isPunching) // Check if "Fire1" is pressed and if not already punching
        {   
            StartCoroutine(PlayPunchAnimation()); // Start the punch animation coroutine
                                                  // Let the PunchDamage script know the punch has started
            punchCollider.GetComponent<PunchDamage>().StartPunch();
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
        
    }


    private IEnumerator PlayPunchAnimation()
    {
        isPunching = true; // Set the flag to true, indicating the animation is playing
        animator.SetTrigger("PunchRight"); // Using the trigger name as set in Animator Controller
        punchDamageScript.StartPunch(); // Let the PunchDamage script know the punch has started

        // Wait for the duration of the animation to play
        yield return new WaitForSeconds(1.0f);

        isPunching = false; // Reset the flag so another punch can be triggered
        punchDamageScript.EndPunch(); // Let the PunchDamage script know the punch has ended
    }
}