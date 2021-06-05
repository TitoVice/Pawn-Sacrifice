using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnMinionBehaviour : MonoBehaviour
{
    public GameObject minionPrefab;
    private GameObject minion;
    private CharacterDeath characterDeath;

    void Start()
    {
        characterDeath = GetComponent<CharacterDeath>();
        minionPrefab = Resources.Load<GameObject>("Prefabs/Abilities/minion");
    }

    public void Spawn() // es fa cada vegada que es passa d'habitacio i aquesta no s'ha completat
    {
        //Pre: ---
        //Post: spawns a minion next to his master

        if (!characterDeath.isDead)
        {
            minion = Instantiate(minionPrefab, RandomPosition(), Quaternion.identity, transform.parent);
        }
    }

    public void Kill() // es fa quan una habitació s'ha acabat
    {
        //Pre: ---
        //Post: destroys the minion at the end of a room

        if (minion != null) { minion.GetComponent<Animator>().SetBool("dead", true); }
    }

    public void GiveMinionEnemies(List<GameObject> enemies)
    {
        if (minion != null) { minion.GetComponent<MinionMovement>().getEnemies(enemies); }
    }

    private Vector3 RandomPosition()
    {
        //Pre: ---
        //Post: gets a valid random position in the NavMesh

        Vector3 randomPos = Random.insideUnitCircle * 0.7f;
        randomPos += transform.position;
        NavMeshHit pos;
        Vector3 finalPos = Vector3.zero;

        bool placed = false;
        while (!placed) { placed = NavMesh.SamplePosition(randomPos, out pos, 3, 1); finalPos = pos.position; }

        return finalPos;
    }
}
