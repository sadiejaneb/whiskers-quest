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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        if (points.Length == 0) return;

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

        if (distanceToPlayer < detectionRange)
        {
            // Player detected, chase the player
            ChasePlayer();
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // Continue patrolling
            GotoNextPoint();
        }

        // Check if within attack range
        if (distanceToPlayer < attackRange)
        {
            // Perform attack (you'll need to implement this)
            AttackPlayer();
        }
    }

    void ChasePlayer()
    {
        agent.destination = player.position;
    }

    void AttackPlayer()
    {
        // Implement your attack logic here
        Debug.Log("Attacking the player!");
    }
}
