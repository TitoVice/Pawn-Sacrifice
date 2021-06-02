using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{
    private GameObject team;
    public GameObject enemies;
    private List<GameObject> listEnemies = new List<GameObject>();
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
            foreach (Transform child in team.transform)
            {
                if (!child.CompareTag("Player")) { child.GetComponent<AgentScript>().getEnemies(listEnemies); }
                child.GetComponent<CharacterDeath>().PassRoom();
                if (child.GetComponent<SpawnMinionBehaviour>()) { child.GetComponent<SpawnMinionBehaviour>().Spawn(); }
                foreach (Transform grandchild in child.transform)
                {
                    if (grandchild.CompareTag("Shield")) { grandchild.GetComponent<ShieldBehaviour>().enterRoom(); }
                }
            }
        }
    }

    public void getTeam(GameObject teamGet)
    {
        team = teamGet;
    }
}
