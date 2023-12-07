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

    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        attackAudioSource = audioSources.Length > 0 ? audioSources[1] : gameObject.AddComponent<AudioSource>();
        damageAudioSource = gameObject.AddComponent<AudioSource>();
        deathAudioSource = gameObject.AddComponent<AudioSource>();

        animator = GetComponent<Animator>();
        punchDamageScript = punchCollider.GetComponent<PunchDamage>();

        collectibleScript = FindObjectOfType<SimpleCollectibleScript>();
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
            if (hasCollectedStar)
            {
                ApplyInstantKill();
            }
            else
            {
                StartCoroutine(PlayPunchAnimation());
                punchCollider.GetComponent<PunchDamage>().StartPunch();
            }
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

    void ApplyInstantKill()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Destroy(collider.gameObject);
            }
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
        punchDamage = newDamage;
        hasCollectedStar = true;
    }
}
