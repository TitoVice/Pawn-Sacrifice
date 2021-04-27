using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveScript : MonoBehaviour
{
    public Transform target = null;
    public Vector3 posToGo;
    public NavMeshAgent agent;
    public Animator animator;
    private SpriteRenderer sprite;

    void Awake()
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

    public virtual void getTarget(Transform objective)
    {
        //Pre: known true if the position is where it has to go, false if it's has to be searched an objective
        //Post: search the nearest target
        
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
        if (target == null) { getTarget(null); }
        agent.SetDestination(target.position);

        lookDirection(target.position);
    }

    public void lookDirection(Vector3 look)
    {
        //Pre: ---
        //Post: sets the sprite looking right or left

        float direction = getDirection(transform.position, look);
        
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
