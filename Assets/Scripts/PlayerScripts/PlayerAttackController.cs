using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    private Animator animator;
    private bool isPunching = false;
    public int punchDamage = 33;
    public GameObject punchCollider;
    private float attackRange = 2.0f; // Adjust this value to match your attack range

    private PunchDamage punchDamageScript;
    private AudioSource attackAudioSource;
    private AudioSource damageAudioSource;
    private AudioSource deathAudioSource;
    public AudioClip deathSound;
    public AudioClip attackSound;
    public AudioClip damageSound;

    private bool hasCollectedStar = false;
    private float starCollectDuration = 10.0f;
    private float starEffectTimer = 0.0f;

    private SimpleCollectibleScript collectibleScript;
    private int originalPunchDamage;

    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        attackAudioSource = audioSources.Length > 0 ? audioSources[1] : gameObject.AddComponent<AudioSource>();
        damageAudioSource = gameObject.AddComponent<AudioSource>();
        deathAudioSource = gameObject.AddComponent<AudioSource>();

        animator = GetComponent<Animator>();
        punchDamageScript = punchCollider.GetComponent<PunchDamage>();

        collectibleScript = FindObjectOfType<SimpleCollectibleScript>();

        originalPunchDamage = punchDamage; // store original damage
    }

    void Update()
    {
        if (hasCollectedStar)
        {
            starEffectTimer += Time.deltaTime;
            if (starEffectTimer >= starCollectDuration)
            {
                hasCollectedStar = false;
                starEffectTimer = 0.0f;
            }
            else
            {
                hasCollectedStar = true;
            }
        }

        if (Input.GetButtonDown("Fire1") && !isPunching)
        {
                StartCoroutine(PlayPunchAnimation());
                punchCollider.GetComponent<PunchDamage>().StartPunch();
        }

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
        isPunching = true;
        animator.SetTrigger("PunchRight");
        punchDamageScript.StartPunch();

        yield return new WaitForSeconds(1.0f);

        isPunching = false;
        punchDamageScript.EndPunch();
    }

    public void UpdatePlayerDamage(int newDamage)
    {
        Debug.Log("Updating player damage to: " + newDamage);
        punchDamage = newDamage;
        Debug.Log("Player damage now: " + punchDamage); // Additional Debug
        hasCollectedStar = true;
        starEffectTimer = 0.0f;
    }

    public void RevertPlayerDamage()
    {
        punchDamage = originalPunchDamage;
        hasCollectedStar = false;
        Debug.Log("Player damage reverted to " + originalPunchDamage);
    }
    public int GetCurrentPunchDamage()
    {
        return punchDamage;
    }
}
