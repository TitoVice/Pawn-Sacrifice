using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentScript : MonoBehaviour
{
    private Transform target;
    private Transform player;
    public bool targetIsPlayer = true;
    private List<GameObject> listEnemies = new List<GameObject>();
    private NavMeshAgent agent;
    private SpriteRenderer sprite;
    public string characterName;
    public bool ranged = false;
    public LayerMask layerMask;
    private float searchTimer = 0.0f;
    private float searchCooldown = 0.5f;

    void Start()
    {
        Transform parent = transform.parent;
        foreach (Transform child in parent) 
        { 
            if (child.CompareTag("Player")) { player = child; break; }
        }
        target = player;

        agent = GetComponent<NavMeshAgent>();
        sprite = GetComponent<SpriteRenderer>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);
        if(GetComponent<NavMeshAgent>().enabled == true)
        {
            searchTimer += Time.deltaTime;
            if (target == null || searchTimer >= searchCooldown)
            {
                getTarget();
                searchTimer = 0.0f;
            }

            setDestination();

            if (target.position.x - transform.position.x > 0.0f) { sprite.flipX = false; } //flip sprite to the left
            else { sprite.flipX = true; } //flip sprite to the right
        }
    }

    private void getTarget()
    {
        //Pre: ---
        //Post: sets the target for te IA to follow/attack

        float minDistance = 99999.0f;
        
        if (listEnemies == null || listEnemies.Count == 0) { target = player; targetIsPlayer = true; } //no enemies, follow player
        else //select nearest enemy
        {
            for (int i = 0; i < listEnemies.Count; i++)
            {
                if (listEnemies[i] != null)
                {
                    float distance = Vector3.Distance(listEnemies[i].transform.position, transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        target = listEnemies[i].transform;
                        targetIsPlayer = false;
                    }
                }
            }
        }

        if (targetIsPlayer) { agent.stoppingDistance = 3; }
        else { agent.stoppingDistance = 0; }
    }

    private void setDestination()
    {
        //Pre: ---
        //Post: sets the destination

        Vector3 destiny = transform.position;

        if (!ranged) { destiny = target.position; }
        else
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(transform.position, (target.position - transform.position), Mathf.Infinity, layerMask);
            
            if (hit.collider != null && !hit.transform.CompareTag("Enemy"))//raycasts to the target, if the path is clear to shoot stay still, if not, move
            {
                destiny = target.position;//it will be moving for the next searchCooldown period
            }
        }
        if (Vector3.Distance(transform.position, target.position) <= 1 && !targetIsPlayer) //flees from the enemy
        {  
            Vector3 dirToEnemy = transform.position - target.transform.position;
            Vector3 newPos = transform.position + dirToEnemy;
            newPos.z = transform.position.z;
            destiny = newPos;
        }
        agent.SetDestination(destiny);
    }

    public void getEnemies(List<GameObject> enemies)
    {
        listEnemies = enemies;
        if (GetComponent<SpawnMinionBehaviour>()) 
        {
            GetComponent<SpawnMinionBehaviour>().GiveMinionEnemies(listEnemies);
        }
    }

    public Vector3 giveTarget()
    {
        return target.position;
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
