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
    public float detectionRadius = 6f;
    public float roamRadius = 7f;
    public float roamWaitTime = 1.2f;

    private NavMeshAgent agent;
    private float roamTimer;

    private enum State
    {
        Roaming,
        Waiting,
        Chasing
    }

    private State currentState;

    public Volume screenEffect;
    private Vignette vignette;
    private FilmGrain filmGrain;
    private float intensity = 0f;
    private float goalIntensity = 0f;
    private float fadeSpeed = 2f;
    private float effectIntesnity;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Roaming;
        SetNewRoamTarget();

        //Gets vignette reference
        if (screenEffect.profile.TryGet(out vignette))
        {
            vignette.intensity.value = 0f;
            Debug.Log("Got Vignette");
        }

        //Gets film grain reference
        if (screenEffect.profile.TryGet(out filmGrain))
        {
            filmGrain.intensity.value = 0f;
            Debug.Log("Got Grain");
        }

        ResetEffects();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        intensity = Mathf.MoveTowards(intensity, goalIntensity, fadeSpeed * Time.deltaTime); //Moves intensity value to goal

        //Sets values in global volume
        if (vignette != null && filmGrain != null)
        {
            vignette.intensity.Override(intensity);

            float jitter = Random.Range(-0.05f, 0.05f);
            filmGrain.intensity.Override(Mathf.Clamp01(intensity + jitter));

        }
        else
        {
            Debug.Log("Effects are null");
        }
            

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

            ResetEffects();

            return;
        }

        agent.SetDestination(player.position);

        float rangeDistance = Mathf.Clamp(distanceToPlayer, 1f, detectionRadius); //Calculates distance within detection radius

        effectIntesnity = 1f - ((rangeDistance - 1f) / (detectionRadius - 1f)); //Converts distance to usable range

        goalIntensity = Mathf.Lerp(0f, 0.8f, effectIntesnity); //Calculates the new effect intensity


        SetMotorSpeeds(0.01f, 0.05f); //Controller rumble
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

    public void ResetEffects()
    {
        StopRumble();
        goalIntensity = 0f;
    }
}