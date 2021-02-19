using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamWorldInteraction : MonoBehaviour
{

    public GameObject[] team; //every member of the team

    public int[] spawnPosition = { 0, 0 }; //spawn position on the level

    void Start()
    {
        for (int i = 0; i < team.Length; i++)
        {
            GameObject character = Instantiate(team[i], new Vector3(0, 0, 0), Quaternion.identity, transform);
            team[i] = character;
        }
    }
    public void spawn(int x, int y)
    {
        spawnPosition[0] = x;
        spawnPosition[1] = y;
        
        for (int i = 0; i < team.Length; i++)
        {
            team[i].GetComponent<PlayerMovement>().spawn(spawnPosition);
        }
    }
}
