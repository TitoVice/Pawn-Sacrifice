using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargerMovementScript : EnemyMoveScript
{
    public float walkingSpeed = 4.0f;
    public float chargingSpeed = 12.0f;
    private Vector3 selectedPosition;
    private bool charging = false, walking = false;
    public LayerMask layerMask;
    private float waitTimer = 0.0f;
    private float waitTime = 0.5f;

    public override void movement(float time)
    {
        if (characters.Count > 0)
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
                waitTimer += time;
                if (waitTimer >= waitTime) { waitTimer = 0.0f; getTarget(null); }
            }

            lookDirection(selectedPosition);
        }
    }

    public override void getTarget(Transform objective)
    {
        float minDistance = 1000.0f;
        bool selected = false;
        target = null;

        if (characters.Count > 0)
        {
            foreach (GameObject player in characters)
            {
                if (player != null)
                {
                    foreach (Transform child in player.transform)
                    {
                        if (child.CompareTag("HitDetector"))
                        {
                            float distance = Vector3.Distance(transform.position, player.transform.position);
                            if (distance < minDistance) 
                            {
                                RaycastHit2D hit;
                                hit = Physics2D.Raycast(transform.position, (player.transform.position - transform.position), Mathf.Infinity, layerMask);
                                
                                if (hit.collider != null && !hit.transform.CompareTag("Background") && !hit.transform.CompareTag("Enemy"))//raycasts directly to a player or companion
                                {
                                    selected = true;
                                    charging = true;
                                    walking = false;
                                    selectedPosition = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
                                    minDistance = distance;
                                    target = player.transform;
                                }
                            }
                        }
                        else if (child.CompareTag("Minion"))
                        {
                            float distance = Vector3.Distance(transform.position, player.transform.position);
                            if (distance < minDistance) 
                            {
                                RaycastHit2D hit;
                                hit = Physics2D.Raycast(transform.position, (player.transform.position - transform.position), Mathf.Infinity, layerMask);
                                
                                if (hit.collider != null && !hit.transform.CompareTag("Background") && !hit.transform.CompareTag("Enemy"))//raycasts directly to a player or companion
                                {
                                    selected = true;
                                    charging = true;
                                    walking = false;
                                    selectedPosition = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
                                    minDistance = distance;
                                    target = player.transform;
                                }
                            }
                        }
                    }
                }
                
            }
            if (!selected && !walking) 
            {
                selectedPosition = RandomPosition();
                walking = true;
            }
        }
    }

    private Vector3 RandomPosition()
    {
        //Pre: ---
        //Post: gets a valid random position in the NavMesh
        
        Vector3 randomPos = Random.insideUnitCircle;// * 0.5f;
        
        randomPos += transform.position;
        Debug.DrawLine(transform.position, randomPos, Color.red, 2.0f);
        NavMeshHit pos;
        Vector3 finalPos = Vector3.zero;

        if (NavMesh.SamplePosition(randomPos, out pos, 3, 1)) { finalPos = pos.position; }

        return finalPos;
    }
}
