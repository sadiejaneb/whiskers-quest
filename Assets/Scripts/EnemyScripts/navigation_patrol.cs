using UnityEngine;
using UnityEngine.AI;

public class navigation_patrol : MonoBehaviour
{
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;

    public Transform player; // Reference to the player
    public float detectionRange = 10f; // Range within which the player is detected
    public float attackRange = 2f; // Range within which the enemy can attack
    private Animator animator;
    private float attackTimer = 0f;
    public float minAttackDelay = 3f; // Minimum delay between attacks
    public float maxAttackDelay = 15f; // Maximum delay between attacks
    private bool isReadyToAttack = false;
    private const float rotSpeed = 20f;
    public float stoppingDistance;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        animator = GetComponent<Animator>();
        GotoNextPoint();
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
        agent.destination = player.position;
    }

    void AttackPlayer()
    {
        Debug.Log("Attacking the player!");
        animator.SetTrigger("Attack");
    }
}
