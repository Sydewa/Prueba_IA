using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Script : MonoBehaviour
{
    enum State {Patrolling, Chasing, Attack, Wait}
    
    State currentState;
    private NavMeshAgent agent;


    public Transform[] destinationPoints;
    int destinationIndex;

    [SerializeField]Transform player;
    [SerializeField]float visionRange;
    [SerializeField]float attackRange;

    [SerializeField]float waitingTime;
    float elapsedTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    
    void Start()
    {
        currentState = State.Patrolling;
        destinationIndex = 0;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
            break;

            case State.Wait:
                Waiting();
            break;

            case State.Chasing:
                Chase();
            break;

            case State.Attack:
                Debug.Log("Attack");
                currentState = State.Patrolling;
            break;

            default:
                currentState = State.Patrolling;
            break;
        }
    }


//------------------------------

    void Patrol()
    {
        agent.destination = destinationPoints[destinationIndex].position;

        if(Vector3.Distance(transform.position, destinationPoints[destinationIndex].position) < 1)
        {
            if(destinationIndex == (destinationPoints.Length - 1))
            {
                destinationIndex = 0;
                currentState = State.Wait;
                
            }
            else
            {
                destinationIndex ++;
                currentState = State.Wait;
            }
            
        }
        
        if(Vector3.Distance(transform.position, player.position) < visionRange)
        {
            currentState = State.Chasing;
        }
    }

    void Chase()
    {
        agent.destination = player.position;
        if(Vector3.Distance(transform.position, player.position) > visionRange)
        {
            currentState = State.Patrolling;
        }
        if(Vector3.Distance(transform.position, player.position) < attackRange)
        {
            currentState = State.Attack;
        }
    }

    void Waiting()
    {
        
        elapsedTime += Time.deltaTime;
        

        if(elapsedTime >= waitingTime)
        {
            currentState = State.Patrolling;
            elapsedTime = 0;
        }
    }

    //--------------

    private void OnDrawGizmos() 
    {
        foreach(Transform point in destinationPoints)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(point.position, 1);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
