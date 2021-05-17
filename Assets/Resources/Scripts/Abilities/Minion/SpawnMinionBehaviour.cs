using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnMinionBehaviour : MonoBehaviour
{
    public GameObject minionPrefab;
    private GameObject minion;

    void Start()
    {
        minionPrefab = Resources.Load<GameObject>("Prefabs/Abilities/minion");
    }

    public void Spawn() // es fa cada vegada que es passa d'habitacio i aquesta no s'ha completat
    {
        //Pre: ---
        //Post: spawns a minion next to his master

        minion = Instantiate(minionPrefab, RandomPosition(), Quaternion.identity, transform);
    }

    public void Kill() // es fa quan una habitació s'ha acabat
    {
        //Pre: ---
        //Post: destroys the minion at the end of a room

        if (minion != null) { Destroy(minion); }
    }

    private Vector3 RandomPosition()
    {
        //Pre: ---
        //Post: gets a valid random position in the NavMesh

        Vector3 randomPos = Random.insideUnitCircle * 0.5f;
        randomPos += transform.position;
        NavMeshHit pos;
        Vector3 finalPos = Vector3.zero;

        if (NavMesh.SamplePosition(randomPos, out pos, 3, 1)) { finalPos = pos.position; }

        return finalPos;
    }
}
