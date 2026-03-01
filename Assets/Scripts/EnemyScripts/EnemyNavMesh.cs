//IEnumerator library
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//Global Volume library
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
//Controller library
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;

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
    private FilmGrain filmGrain;
    private float intensity = 0f;
    private float goalIntensity = 0f;
    private float fadeSpeed = 2f;
    private float effectIntesnity;

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

        //Gets film grain reference
        if (screenEffect.profile.TryGet(out filmGrain))
        {
            filmGrain.intensity.value = 0f;
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

            SetMotorSpeeds(0.005f, 0.01f); //Controller rumble

            //Rumble option 2
            //StartCoroutine(SetRumble(0.01f, 0.06f)); 

            float rangeDistance = Mathf.Clamp(distanceToPlayer, 1f, 10f); //Calculates distance within detection radius

            effectIntesnity = 1f - ((rangeDistance - 1f) / (10f -1f)); //Converts distance to usable range

            goalIntensity = Mathf.Lerp(0f, 0.8f, effectIntesnity); //Calculates the new effect intensity 
        }
        else
        {
            // Roam
            roamTimer -= Time.deltaTime;

            if (roamTimer <= 0f || agent.remainingDistance < 0.5f)
            {
                SetNewRoamTarget();
            }

            StopRumble();

            goalIntensity = 0f; //Resets goal intensity 
        }


        intensity = Mathf.MoveTowards(intensity, goalIntensity, fadeSpeed * Time.deltaTime); //Moves intensity value to goal

        //Sets values in global volume
        if (vignette != null && filmGrain != null)
        {
            vignette.intensity.Override(intensity);

            float jitter = Random.Range(-0.05f, 0.05f);
            filmGrain.intensity.Override(Mathf.Clamp01(intensity + jitter));

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

    //Sets controller rumble
    void SetMotorSpeeds(float lowFrequency, float highFrequency) 
    {
        if (Gamepad.current != null)
            Gamepad.current.SetMotorSpeeds(lowFrequency, highFrequency);
    }

    //IEnumerator rumble routine
    /*public IEnumerator SetRumble(float lowFrequency, float highFrequency)
    {
        if (Gamepad.current != null)
            Gamepad.current.SetMotorSpeeds(lowFrequency, highFrequency);

        yield return new WaitForSeconds(60f);
        StopRumble();
    }*/

    //Stops controller rumble
    void StopRumble()
    {
        if (Gamepad.current != null)
            InputSystem.ResetHaptics();
    }
}