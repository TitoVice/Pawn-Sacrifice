using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{
    private GameObject team;
    public GameObject enemies;
    private List<GameObject> listEnemies = new List<GameObject>();
    private List<AgentScript> companions = new List<AgentScript>();
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
                if (!child.CompareTag("Player")) {  companions.Add(child.GetComponent<AgentScript>());  companions[i].getEnemies(listEnemies); i++; }
                child.GetComponent<CharacterDeath>().PassRoom();
                if (child.GetComponent<SpawnMinionBehaviour>()) { child.GetComponent<SpawnMinionBehaviour>().Spawn(); }
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

        foreach (AgentScript agent in companions)
        {
            agent.getEnemies(listEnemies);
        }
    }

    public void getTeam(GameObject teamGet)
    {
        team = teamGet;
    }
}
