using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class EnemyNavMesh : MonoBehaviour
{

    [SerializeField] private Transform movePositionTransform;

    private NavMeshAgent navMeshAgent;
    
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

        void Update()
    {
        navMeshAgent.destination = movePositionTransform.position;
    }
}
