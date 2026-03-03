using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 10f;
    public float roamRadius = 5f;
    public float roamWaitTime = 3f;

    private NavMeshAgent agent;
    private float roamTimer;

    private enum State
    {
        Roaming,
        Waiting,
        Chasing
    }

    private State currentState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Roaming;
        SetNewRoamTarget();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Roaming:
                HandleRoaming(distanceToPlayer);
                break;

            case State.Waiting:
                HandleWaiting(distanceToPlayer);
                break;

            case State.Chasing:
                HandleChasing(distanceToPlayer);
                break;
        }
    }

    void HandleRoaming(float distanceToPlayer)
    {
        if (CanSeeAndReachPlayer(distanceToPlayer))
        {
            currentState = State.Chasing;
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentState = State.Waiting;
            roamTimer = roamWaitTime;
            agent.isStopped = true;
        }
    }

    void HandleWaiting(float distanceToPlayer)
    {
        if (CanSeeAndReachPlayer(distanceToPlayer))
        {
            agent.isStopped = false;
            currentState = State.Chasing;
            return;
        }

        roamTimer -= Time.deltaTime;

        if (roamTimer <= 0f)
        {
            agent.isStopped = false;
            SetNewRoamTarget();
            currentState = State.Roaming;
        }
    }

    void HandleChasing(float distanceToPlayer)
    {
        if (!CanSeeAndReachPlayer(distanceToPlayer))
        {
            currentState = State.Roaming;
            SetNewRoamTarget();
            return;
        }

        agent.SetDestination(player.position);
    }

    void SetNewRoamTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    bool CanSeeAndReachPlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > detectionRadius)
            return false;

        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(player.position, path))
        {
            return path.status == NavMeshPathStatus.PathComplete;
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, roamRadius);
    }
}