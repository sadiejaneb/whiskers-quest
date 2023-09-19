using System.Collections;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    private Animator animator;
    private bool isPunching = false; // flag to check if the punch animation is playing

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

    private IEnumerator PlayPunchAnimation()
    {
        isPunching = true; // Set the flag to true, indicating the animation is playing
        animator.SetTrigger("PunchRight"); // Using the trigger name as set in Animator Controller

        // Wait for the duration of the animation to play
        yield return new WaitForSeconds(1.0f);

        isPunching = false; // Reset the flag so another punch can be triggered
    }
}