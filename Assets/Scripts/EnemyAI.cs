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

    private Transform _playerTransform;
    //Puntos patrulla
    [SerializeField] Transform[] _patrolPoints;

    //Detección
    [SerializeField] float _visionRange = 10;

    [SerializeField] float _visionAngle = 120;
    private Vector3 _playerLastPosition;

    
    void Awake()
    {
        _AiAgent = GetComponent<NavMeshAgent>();

        _playerTransform = GameObject.FindWithTag("Player").transform;
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
        if(OnRange())
        {
            currentState = EnemyState.Chasing;
        }
        if(_AiAgent.remainingDistance < 0.3f)
        {
            SetRandomPatrolPoint();
        }
        
    }

    void Chase()
    {
        if(!OnRange())
        {
            currentState = EnemyState.Patroling;
        }
        _AiAgent.destination = _playerTransform.position; 
    }

    void Search()
    {

    }

    bool OnRange()
    {
       
        Vector3 directionToPlayer = _playerTransform.position - transform.position;

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

        if(_playerTransform.position == _playerLastPosition)
        {
            return true;
        }
        
        
        
        if(distanceToPlayer > _visionRange)
        {
            return false;
        }

        if(angleToPlayer > _visionAngle * 0.5f)
        {
            return false;
        }

        RaycastHit hit;
        if(Physics.Raycast(transform.position, directionToPlayer, out hit, distanceToPlayer))
        {
            if(hit.collider.CompareTag("Player"))
            {
                _playerLastPosition = _playerTransform.position;

                return true;
            }

            else
            {
                return false;
            }
        }

        return true;





    
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

         Gizmos.color = Color.yellow;
         Gizmos.DrawWireSphere(transform.position, _visionRange);

        Gizmos.color = Color.green;

        Vector3 fovLine1 = Quaternion.AngleAxis(_visionAngle * 0.5f, transform.up) * transform.forward * _visionRange;
        Vector3 fovLine2 = Quaternion.AngleAxis(-_visionAngle * 0.5f, transform.up) * transform.forward * _visionRange;

        Gizmos.DrawLine(transform.position, transform.position + fovLine1);
        Gizmos.DrawLine(transform.position, transform.position + fovLine2);
        
        
    }
}
