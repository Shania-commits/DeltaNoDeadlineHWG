using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 10f;
    public float roamRadius = 5f;
    public float roamWaitTime = 3f;

    private NavMeshAgent agent;
    private Vector3 roamTarget;
    private float roamTimer = 0f;

    public Volume screenEffect;
    private Vignette vignette;

    void Start()
    {
        // Ensure player reference is set
        agent = GetComponent<NavMeshAgent>();
        SetNewRoamTarget();

        //Gets vignette reference
        if (screenEffect.profile.TryGet(out vignette))
        {
            vignette.intensity.value = 0f;
        }
    }

    void Update()
    {
        // Check distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        

        if (distanceToPlayer <= detectionRadius && CanReachPlayer())
        {
            // Chase the player if close and reachable
            agent.SetDestination(player.position);

            float rangeDistance = Mathf.Clamp(distanceToPlayer, 1f, 10f); //Calculates distance within detection radius

            float vignetteIntesnity = 1f - ((rangeDistance - 1f) / (10f -1f)); //Converts distance to usable range

            if (vignette != null)
            {
                vignette.intensity.value = Mathf.Lerp(0f, 0.8f, vignetteIntesnity); //Increases vignette on distance
            }
        }
        else
        {
            // Roam
            roamTimer -= Time.deltaTime;

            if (roamTimer <= 0f || agent.remainingDistance < 0.5f)
            {
                SetNewRoamTarget();
            }
        }
    }

    void SetNewRoamTarget()
    {
        // Pick a random point within the roam radius
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;
        NavMeshHit hit;

        // Ensure the random point is on the NavMesh
        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
        {
            roamTarget = hit.position;
            agent.SetDestination(roamTarget);
        }

        roamTimer = roamWaitTime;
    }
    // Check if the player is reachable via NavMesh
    bool CanReachPlayer()
    {
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(player.position, path))
        {
            return path.status == NavMeshPathStatus.PathComplete;
        }
        return false;
    }

    // Visualize detection and roam areas in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, roamRadius);
    }
}