using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargerMovementScript : EnemyMoveScript
{

    public GameObject[] characters; //al final sera l'objecte team passat per les portes, agafa els fills(jugadors)
    public float walkingSpeed = 4.0f;
    public float chargingSpeed = 15.0f;
    private Vector3 selectedPosition;
    private bool charging = false, walking = false;
    public LayerMask layerMask;
    private float timer = 0.0f;
    private float waitTime = 1.0f;

    public override void movement(float time)
    {
        if (walking)
        {
            agent.speed = walkingSpeed;
            agent.SetDestination(selectedPosition);
            getTarget(null);
            if (Vector3.Distance(transform.position, selectedPosition) < 1f) { walking = false; }
        }
        else if (charging)
        {
            agent.speed = chargingSpeed;
            agent.SetDestination(selectedPosition);
            if (Vector3.Distance(transform.position, selectedPosition) < 1f) { charging = false; }
        }

        if (!walking && !charging) 
        { 
            timer += time;
            if (timer >= waitTime) { timer = 0.0f; getTarget(null);  }
        }

        lookDirection(selectedPosition);
    }

    public override void getTarget(Transform objective)
    {
        float minDistance = 1000.0f;
        bool selected = false;
        target = null;

        foreach (GameObject player in characters)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < minDistance)
            {
                RaycastHit2D hit;
                hit = Physics2D.Raycast(transform.position, (player.transform.position - transform.position), Mathf.Infinity, layerMask);
                
                if (hit.collider != null && !hit.transform.CompareTag("Background") && !hit.transform.CompareTag("Enemy"))//raycasts directly to a player or companion
                {
                    selected = true;
                    charging = true;
                    walking = false;
                    selectedPosition = new Vector3(player.transform.position.x*1.2f, player.transform.position.y*1.2f, player.transform.position.z);
                    //Debug.DrawLine(transform.position, selectedPosition, Color.red, 5.0f);
                }
            }
        }
        if (!selected && !walking) 
        {
            selectedPosition = RandomPosition();
            walking = true;
        }
    }

    private Vector3 RandomPosition()
    {
        //Pre: ---
        //Post: gets a valid random position in the NavMesh

        Vector3 randomPos = Random.insideUnitCircle * 3;
        randomPos += transform.position;
        NavMeshHit pos;
        Vector3 finalPos = Vector3.zero;

        if (NavMesh.SamplePosition(randomPos, out pos, 3, 1)) { finalPos = pos.position; }

        return finalPos;
    }
}
