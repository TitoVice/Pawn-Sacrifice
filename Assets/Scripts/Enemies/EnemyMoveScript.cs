using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveScript : MonoBehaviour
{
    public Transform target = null;
    private Vector3 posToGo;
    private NavMeshAgent agent;
    public Animator animator;
    private SpriteRenderer sprite;

    void Awake()
    {
        if (target == null) { getTarget(false, Vector2.zero); }

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
            //if (target == null) { getTarget(false, Vector2.zero); }
            movement(Time.deltaTime);
        }
    }

    public void getTarget(bool known, Vector3 objective)
    {
        //Pre: known true if the position is where it has to go, false if it's has to be searched an objective
        //Post: search the nearest target
        
        if (!known)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else
        {
            target = null;
            posToGo = objective;
        }
    }

    public virtual void movement(float time)
    {
        if (target != null) { posToGo = target.position; }
        agent.SetDestination(posToGo);

        
        float direction = getDirection(transform.position, posToGo);
        
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
