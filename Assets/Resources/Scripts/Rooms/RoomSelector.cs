using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSelector : MonoBehaviour
{
    public GameObject[] rooms;
    public GameObject specialRoom; //room to be used as end or initial room

    public GameObject selectRoom(bool special)
    {
        //Pre: ---
        //Post: if special true, return the special room, else returns a random room

        if (special) { return specialRoom; }
        else
        {
            int random = Random.Range(0, rooms.Length - 1);
            return rooms[random];
        }
    }
}
