using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{
    private GameObject team;
    public GameObject enemies;
    private List<GameObject> listEnemies = new List<GameObject>();
    private List<GameObject> companions = new List<GameObject>();
    public GameObject doors;
    private bool roomFinished = false;


    void Start()
    {
        foreach (Transform child in enemies.transform)
        {
            listEnemies.Add(child.gameObject);
        }
    }

    void Update()
    {
        if (!roomFinished && enemies.transform.childCount == 0)
        {
            roomFinished = true;
            foreach (Transform child in doors.transform)
            {
                child.GetComponent<InRoomBehaviour>().OpenDoor();
            }
            if (team != null)
            {
                foreach (Transform child in team.transform)
                {
                    if (child.GetComponent<SpawnMinionBehaviour>()) { child.GetComponent<SpawnMinionBehaviour>().Kill(); }
                }
            }
        }
    }

    public void enterRoom()
    {
        if (!roomFinished)
        {
            int i = 0;
            foreach (Transform child in team.transform)
            {
                if (child.GetComponent<SpawnMinionBehaviour>()) { child.GetComponent<SpawnMinionBehaviour>().Spawn(); }
                if (!child.CompareTag("Player")) 
                {  
                    companions.Add(child.gameObject);
                    if (companions[i].GetComponent<AgentScript>()) { companions[i].GetComponent<AgentScript>().getEnemies(listEnemies); } //companions, it already passes the info to the minion
                    i++; 
                }
                else 
                {
                    if (child.GetComponent<SpawnMinionBehaviour>()) { child.GetComponent<SpawnMinionBehaviour>().GiveMinionEnemies(listEnemies); } //if the player spawns a minion
                }
                
                foreach (Transform grandchild in child.transform)
                {
                    if (grandchild.CompareTag("Shield")) { grandchild.GetComponent<ShieldBehaviour>().enterRoom(); }
                }
            }
        }
    }

    public void EnemyListChange()
    {
        //Pre: ---
        //Post: an enemy has died or spawned, so the list in every companion has to be uploaded

        listEnemies = new List<GameObject>();
        foreach (Transform child in enemies.transform)
        {
            listEnemies.Add(child.gameObject);
        }

        foreach (GameObject agent in companions)
        {
            if (agent != null)
            {
                if (agent.GetComponent<AgentScript>()) { agent.GetComponent<AgentScript>().getEnemies(listEnemies); }
            }
        }

        if (listEnemies.Count == 0)
        {
            foreach (Transform child in team.transform)
            {
                if (child.GetComponent<CharacterDeath>()) { child.GetComponent<CharacterDeath>().PassRoom(); }
            }
        }
    }

    public void getTeam(GameObject teamGet)
    {
        team = teamGet;
    }
}
