using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveScript : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    public Animator animator;
    private SpriteRenderer sprite;

    void Start()
    {
        if (target == null) { getTarget(null); }

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

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

        float direction = getDirection(transform.position, target.position);
        
        if (direction > 0) { sprite.flipX = false; } //enemy looking to right
        else if (direction < 0) { sprite.flipX = true; } //enemy looking to left
    }

    public void Immobilize()
    {
        GetComponent<NavMeshAgent>().enabled = false;
    }

    public void Mobilize()
    {
        GetComponent<NavMeshAgent>().enabled = true;
    }

    private float getDirection(Vector3 itself, Vector3 target)
    {
        //Pre: 2 positions
        //Post: return negative if objective is left from itself, positive if it's in the right, 0 if they are aligned

        return target.x - itself.x;

    }
}
