using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamWorldInteraction : MonoBehaviour
{

    public GameObject[] team; //every member of the team
    public List<GameObject> teamList;
    private int spawnSeparation = 1;
    private int tpDistance = 2;

    void Start()
    {
        teamList = new List<GameObject>();

        for (int i = 0; i < team.Length; i++)
        {
            teamList.Add(Instantiate(team[i], new Vector3(0, 0, 0), Quaternion.identity, transform));
        }
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
        teamList.Remove(characterToDelete);
        Destroy(characterToDelete);
    }

    public void roomPass(Vector3 doorPos, bool isVertical, bool isTop, bool isRight)
    {
        //Pre: valid position
        //Post: transports the companions to another room
        
        int side = 1, order = 1;
        if (isTop || isRight) { side *= -1; }

        for (int i = 0; i < teamList.Count; i++)
        {
            if (!team[i].CompareTag("Player"))
            {
                Vector3 newPos;
                teamList[i].GetComponent<AgentScript>().Immobilize(); //desactivate the navmeshAgent in order to transport the companion

                if (i%2 == 0) { order *= -1; }

                if (isVertical) { newPos = new Vector3(doorPos.x + (tpDistance*side), doorPos.y + ((i+1)*0.7f*order), 0); }
                else { newPos = new Vector3(doorPos.x + ((i+1)*0.7f*order), doorPos.y + (tpDistance*side), 0); }

                teamList[i].transform.position = newPos;
                teamList[i].GetComponent<AgentScript>().Mobilize();
            }

            
        }
    }
}
