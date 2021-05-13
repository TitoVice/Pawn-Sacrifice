using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionMovement : MonoBehaviour
{
    public GameObject[] enemies; //al final sera l'objecte team passat per les portes, agafa els fills(jugadors)
    public Transform target = null;
    public NavMeshAgent agent;
    public Animator animator;
    private SpriteRenderer sprite;

    private float timer = 2.5f;
    private float targetTimer = 2.0f;

    void Awake()
    {
        if (target == null) { getTarget(); }

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

    public virtual void getTarget()
    {
        //Pre: known true if the position is where it has to go, false if it's has to be searched an objective
        //Post: search the nearest target or gets the objective
        
        float minDistance = 1000.0f;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance) 
            {
                minDistance = distance;
                target = enemy.transform;
            }
        }
    }

    public virtual void movement(float time)
    {

        if (timer >= targetTimer) { getTarget(); }
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

    private float getDirection(Vector3 itself, Vector3 target)
    {
        //Pre: 2 positions
        //Post: return negative if objective is left from itself, positive if it's in the right, 0 if they are aligned

        return target.x - itself.x;
    }

    public void Kill()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        agent.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.1f);
    }
}
