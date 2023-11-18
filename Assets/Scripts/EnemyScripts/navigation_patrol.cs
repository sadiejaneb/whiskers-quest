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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        animator = GetComponent<Animator>();
        GotoNextPoint();

        if (attackCollider != null)
    {
        attackCollider.enabled = false; // Ensure the attack collider is disabled at start
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
        Debug.Log("Attacking the player!");
        animator.SetTrigger("Attack");

        // Enable the NavMeshAgent and set its properties for the lunge
        PrepareForLunge();

        StartCoroutine(EnableAttackColliderAtRightMoment());
    }

    void PrepareForLunge()
    {
        agent.isStopped = false;
        agent.stoppingDistance = 0f; // Allow close approach for the lunge
        agent.speed = 15f; // Increase speed for lunge
        agent.acceleration = 30f; // Increase acceleration
        agent.destination = player.position;
    }


    private IEnumerator EnableAttackColliderAtRightMoment()
    {
        // Wait for the right moment in the attack animation to enable the collider
        yield return new WaitForSeconds(0.8f); 
        attackCollider.enabled = true;
        StartCoroutine(DisableAttackColliderAfterDelay());
    }

    private IEnumerator DisableAttackColliderAfterDelay()
    {
        // Wait for the duration of the attack animation
        yield return new WaitForSeconds(0.25f);

        attackCollider.enabled = false;

        // Reset agent properties to original values
        agent.stoppingDistance = stoppingDistance;
        agent.speed = 3.5f; // Reset to original speed, adjust as needed
        agent.acceleration = 8f; // Reset to original acceleration, adjust as needed
        //agent.isStopped = true;

        // Resume chasing or patrolling
        ResumeBehavior();
    }

    private void ResumeBehavior()
    {
        agent.isStopped = false;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceToPlayer < detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            GotoNextPoint();
        }
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
}
