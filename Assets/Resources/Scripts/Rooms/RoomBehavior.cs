using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{
    private GameObject team;
    public GameObject enemies;
    public GameObject doors;
    private bool roomFinished = false;

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
