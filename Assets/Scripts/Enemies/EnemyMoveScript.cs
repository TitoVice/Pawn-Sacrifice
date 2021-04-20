using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveScript : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;

    void Start()
    {
        if (target == null) { getTarget(null); }

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if(GetComponent<NavMeshAgent>().enabled == true)
        {
            movement(Time.deltaTime);
        }
    }

    public void getTarget(Transform objective)
    {
        if (objective == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else
        {
            target = objective;
        }
    }

    public virtual void movement(float time)
    {
        agent.SetDestination(target.position);
    }

    public void Immobilize()
    {
        GetComponent<NavMeshAgent>().enabled = false;
    }

    public void Mobilize()
    {
        GetComponent<NavMeshAgent>().enabled = true;
    }
}
