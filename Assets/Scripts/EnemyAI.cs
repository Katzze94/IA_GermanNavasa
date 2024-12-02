using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyAI : MonoBehaviour
{

    public enum EnemyState
    {
        Patroling,
        Chasing,

        Searching
    }

    public EnemyState currentState;

    private NavMeshAgent _AiAgent;
    
    [SerializeField] Transform[] _patrolPoints;
    
    void Awake()
    {
        _AiAgent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        currentState = EnemyState.Patroling;
        SetRandomPatrolPoint();

    }
    
    
        void Update()
    {
        switch (currentState)
        {
            
            case EnemyState.Patroling:
                Patrol();
            break;
            case EnemyState.Chasing:
            Chase();

            break;

            case EnemyState.Searching:
            Search();
            break;

        }
    }

    void Patrol()
    {
        if(_AiAgent.remainingDistance < 0.3f)
        {
            SetRandomPatrolPoint();
        }
        
    }

    void Chase()
    {

    }

    void Search()
    {

    }

    void SetRandomPatrolPoint()
    {
        _AiAgent.destination =_patrolPoints[Random.Range(0, _patrolPoints.Length)].position;
    }

    void OnDrawGizmos()
    {
         foreach (Transform point in _patrolPoints)
         {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(point.position, 1f);
         }
    }
}
