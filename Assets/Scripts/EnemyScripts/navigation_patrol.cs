using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class navigation_patrol : MonoBehaviour
{
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;

    public Transform player; // Reference to the player
    public float detectionRange = 10f; // Range within which the player is detected
    public float attackRange = 5f; // Range within which the enemy can attack
    private Animator animator;
    private float attackTimer = 0f;
    public float minAttackDelay = 3f; // Minimum delay between attacks
    public float maxAttackDelay = 10f; // Maximum delay between attacks
    private bool isReadyToAttack = false;
    private const float rotSpeed = 20f;
    public float stoppingDistance;
    public int attackDamage;
    public Collider attackCollider;
    public AudioClip movingSound; // Sound to be played when IsMoving
    public AudioClip attackSound; // Sound to be played when attacking
    public AudioClip damageSound; // Sound to play when damaged

    private AudioSource movementAudioSource;
    private AudioSource attackAudioSource;
    private AudioSource damageAudioSource;

    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        movementAudioSource = audioSources[0];
        attackAudioSource = audioSources.Length > 1 ? audioSources[1] : gameObject.AddComponent<AudioSource>();
        damageAudioSource = gameObject.AddComponent<AudioSource>(); // Additional AudioSource for damage sound

        movementAudioSource.clip = movingSound;

        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        animator = GetComponent<Animator>();
        GotoNextPoint();

        if (attackCollider != null)
    {
        attackCollider.enabled = false; // Ensure the attack collider is disabled at start
    }
    }
    public void PlayDamageSound()
    {
        if (damageSound != null)
        {
            damageAudioSource.PlayOneShot(damageSound);
        }
    }

    void GotoNextPoint()
    {
        if (points.Length == 0) return;

        agent.stoppingDistance = 0f; // Reset stopping distance for patrolling

        int newDestPoint = 0;
        do
        {
            newDestPoint = Random.Range(0, points.Length);
        } while (destPoint == newDestPoint);

        destPoint = newDestPoint;
        agent.destination = points[destPoint].position;
    }

    void Update()
    {
        // Check distance to the player
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        InstantlyTurn(agent.destination);

        if (distanceToPlayer < detectionRange)
        {
            // Player detected, chase the player
            ChasePlayer();
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f && this.enabled)
        {
            // Continue patrolling
            GotoNextPoint();
        }

        // Update the IsMoving boolean in the Animator
        animator.SetBool("IsMoving", agent.velocity.magnitude > 0.1f);


        if (!IsDamagedAnimationPlaying())
        {
            // Update attack timer and check if it's time to attack
            if (!isReadyToAttack)
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0f)
                {
                    isReadyToAttack = true;
                    attackTimer = Random.Range(minAttackDelay, maxAttackDelay); // Reset timer for next attack
                }
            }
            else if (distanceToPlayer < attackRange)
            {
                AttackPlayer();
                isReadyToAttack = false; // Reset attack readiness after performing attack
                attackTimer = Random.Range(minAttackDelay, maxAttackDelay); // Reset timer for next attack
            }
        }
        // Play or stop the moving sound based on movement
        if (animator.GetBool("IsMoving"))
        {
            if (!movementAudioSource.isPlaying)
            {
                movementAudioSource.Play();
            }
        }
        else
        {
            if (movementAudioSource.isPlaying)
            {
                movementAudioSource.Stop();
            }
        }
    }
    bool IsDamagedAnimationPlaying()
    {
        // Check if this NPC is a ghost or a bat
        if (gameObject.CompareTag("Ghost") || gameObject.CompareTag("Bat"))
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName("IsDamaged");
        }
        return false; // For other NPCs, always return false
    }
    private void InstantlyTurn(Vector3 destination)
    {
        //When on target -> dont rotate!
        if ((destination - transform.position).magnitude < 0.1f) return;

        Vector3 direction = (destination - transform.position).normalized;
        Quaternion qDir = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, qDir, Time.deltaTime * rotSpeed);
    }

    void ChasePlayer()
    {
        agent.stoppingDistance = stoppingDistance; // Set stopping distance for chasing
        agent.isStopped = false;
        agent.destination = player.position;
    }

    void AttackPlayer()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null && playerHealth.IsAlive)
        {
            Debug.Log("Attacking the player!");
            animator.SetTrigger("Attack");

            // Calculate lunge distance
            float distanceToPlayer = Vector3.Distance(player.position, transform.position);
            float lungeDistance = Mathf.Min(4f, distanceToPlayer - stoppingDistance);

            // Target position for the lunge
            Vector3 lungeTarget = transform.position + (player.position - transform.position).normalized * lungeDistance;

            // Immediately set the agent to lunge towards the player
            agent.isStopped = false;
            agent.stoppingDistance = 0f; // No stopping distance during attack
            agent.speed = 15f; // Increased speed for the lunge
            agent.acceleration = 35f; // Higher acceleration for immediate response
            agent.destination = lungeTarget; // Lunge towards the target position
        }
    }

    public void EnableAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = true;
        }
        
    }
    public void PlayAttackSound()
    {
        if (attackSound != null)
        {
            attackAudioSource.PlayOneShot(attackSound);
        }
    }


    public void DisableAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
        // Reset the stopping distance back to the original value
        agent.stoppingDistance = stoppingDistance;

        
        agent.speed = 3.5f; // Reset to original speed, adjust as needed
        agent.acceleration = 8f; // Reset to original acceleration, adjust as needed
        //agent.isStopped = false; // Make sure the agent can move again
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && attackCollider.enabled)
        {
            Debug.Log("Player hit!");
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ApplyDamage(attackDamage);
            }
        }
    }
    public void StopAllSoundsAndDisableAudioSources()
    {
        // Stop all sounds
        if (movementAudioSource.isPlaying) movementAudioSource.Stop();
        if (attackAudioSource.isPlaying) attackAudioSource.Stop();
        if (damageAudioSource.isPlaying) damageAudioSource.Stop();

        // Disable audio source components
        movementAudioSource.enabled = false;
        attackAudioSource.enabled = false;
        damageAudioSource.enabled = false;
    }
}
