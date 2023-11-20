using System.Collections;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    private Animator animator;
    private bool isPunching = false; // flag to check if the punch animation is playing
    public int punchDamage = 33; // damage to apply when punching
    public GameObject punchCollider; // Assign this in the Inspector to the GameObject with the PunchDamage script

    private PunchDamage punchDamageScript; // Reference to the PunchDamage script
    private AudioSource attackAudioSource;
    private AudioSource damageAudioSource;
    private AudioSource deathAudioSource;
    public AudioClip deathSound; // Sound to be played when dying
    public AudioClip attackSound; // Sound to be played when attacking
    public AudioClip damageSound; // Sound to play when damaged

    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        attackAudioSource = audioSources.Length > 0 ? audioSources[1] : gameObject.AddComponent<AudioSource>();
        damageAudioSource = gameObject.AddComponent<AudioSource>(); // Additional AudioSource for damage sound
        deathAudioSource = gameObject.AddComponent<AudioSource>(); // Additional AudioSource for death sound

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

    public void PlayAttackSound()
    {
        if (attackSound != null)
        {
            attackAudioSource.PlayOneShot(attackSound);
        }
    }
    public void PlayDamageSound()
    {
        if (damageSound != null)
        {
            damageAudioSource.PlayOneShot(damageSound);
        }
    }
    public void PlayDeathSound()
    {
        if (deathSound != null)
        {
            deathAudioSource.PlayOneShot(deathSound);
        }
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