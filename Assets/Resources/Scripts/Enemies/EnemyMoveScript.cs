using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveScript : MonoBehaviour
{
    public List<GameObject> characters; //al final sera l'objecte team passat per les portes, agafa els fills(jugadors)
    public Transform target = null;
    private Vector3 posToGo;
    public NavMeshAgent agent;
    public Animator animator;
    private SpriteRenderer sprite;

    private float timer = 2.5f;
    private float targetTimer = 2.0f;

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
        transform.rotation = new Quaternion(0, 0, 0, 0);
        if(GetComponent<NavMeshAgent>().enabled == true)
        {
            movement(Time.deltaTime);
        }
    }

    public virtual void getTarget(Transform objective)
    {
        //Pre: known true if the position is where it has to go, false if it's has to be searched an objective
        //Post: search the nearest target or gets the objective
        
        if (characters.Count > 0)
        {
            if (objective == null)
            {
                float minDistance = 1000.0f;

                foreach (GameObject player in characters)
                {
                    foreach (Transform child in player.transform)
                    {
                        if (child.CompareTag("HitDetector") && !child.GetComponent<CharacterGetHit>().dead)
                        {
                            float distance = Vector3.Distance(transform.position, player.transform.position);
                            if (distance < minDistance) 
                            {
                                minDistance = distance;
                                target = player.transform;
                            }
                        }
                        else if (child.CompareTag("Minion") && !child.GetComponent<MinionGetHit>().dead)
                        {
                            float distance = Vector3.Distance(transform.position, player.transform.position);
                            if (distance < minDistance) 
                            {
                                minDistance = distance;
                                target = player.transform;
                            }
                        }
                    }
                }
            }
            else
            {
                target = objective;
            }
        }
    }

    public virtual void movement(float time)
    {
        if (characters.Count > 0)
        {
            if (timer >= targetTimer) { getTarget(null); }
            agent.SetDestination(target.position);

            lookDirection(target.position);
        }
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

    public void getCharacters(GameObject team)
    {
        foreach (Transform child in team.transform)
        {
            characters.Add(child.gameObject);
        }
    }
}
