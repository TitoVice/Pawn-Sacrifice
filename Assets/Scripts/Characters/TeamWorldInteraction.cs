using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamWorldInteraction : MonoBehaviour
{

    public GameObject[] team; //every member of the team
    public List<GameObject> teamList;
    private int spawnSeparation = 1;

    void Start()
    {
        teamList = new List<GameObject>();

        for (int i = 0; i < team.Length; i++)
        {
            teamList.Add(Instantiate(team[i], new Vector3(0, 0, 0), Quaternion.identity, transform));
        }
        //Debug.Log(teamList.Count + "-----------");
    }
    public void spawn(int x, int y)
    {
        int[] pos1 = { x, y + spawnSeparation };
        int[] pos2 = { x, y - spawnSeparation };

        int posX = -spawnSeparation;
        
        for (int i = 0; i < teamList.Count; i++)
        {
            if (!team[i].CompareTag("Player"))
            {
                teamList[i].GetComponent<AgentScript>().Immobilize(); //desactivate the navmeshAgent in order to transport the companion
            }

            if (i == 0) { teamList[i].GetComponent<SpawnCharacter>().spawn(pos1); }
            else if (i == teamList.Count - 1) { teamList[i].GetComponent<SpawnCharacter>().spawn(pos2); }
            else
            {
                int[] newPos = { x + posX, y };
                teamList[i].GetComponent<SpawnCharacter>().spawn(newPos);
                posX += spawnSeparation;
            }
            if (!teamList[i].CompareTag("Player"))
            {
                teamList[i].GetComponent<AgentScript>().Mobilize();
            }
        }
    }

    public void Reorganize(GameObject characterToDelete)
    {
        /*GameObject[] aux = new GameObject[team.Length - 1];
        for (int i = 0; i < teamList.Count; i++)
        {
            if (team[i] == null)
            {
                if (i + 1 < team.Length)
                {
                    team[i] = team[i + 1];
                    team[i + 1] = null;
                }
            }
            if (i < team.Length - 1) { aux[i] = team[i]; }
        }
        team = aux;*/

        teamList.Remove(characterToDelete);
        Destroy(characterToDelete);
        //Debug.Log(teamList[1]);
    }
}
