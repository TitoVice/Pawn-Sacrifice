using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDoors : MonoBehaviour
{
    public string doors; //input of 4 0 and 1, XXXX == LRDT. If 1 it has a door in that side
    public int L = 0;
    public int R = 0;
    public int D = 0;
    public int T = 0;
    public int[] position = { 0, 0 };

    public int roomSize;

    public bool start = false;
    public bool deadEnd = false;
    public bool used = false;
    public bool inRoom = false;
    public bool visited = false;

    //variables for big rooms
    public bool bigger = false;
    public string bigName = "";
    public bool bigToSpawn = false;

    public void getPositionSpecial(int[] pos, bool isStart, bool isDeadEnd, bool isBigger, int size)
    {
        position[0] = pos[0];
        position[1] = pos[1];
        start = isStart;
        deadEnd = isDeadEnd;
        bigger = isBigger;
        used = true;

        if (!bigger){ roomSize = size; }

        if (start) { inRoom = true; visited = true; }
    }

    public void getName_Spawn(string name)
    {
        bigName = name;
        spawn = true;
    }

    public void openDoor(int[] pos)
    {
        int x = position[0] - pos[0];
        int y = position[1] - pos[1];

        if (x == 1) { L = 1; }
        else if(x == -1) { R = 1; }
        else
        {
            if(y == 1) { D = 1; }
            else if(y == -1) { T = 1; }
        }
    }

    public void getValues(RoomDoors copyRoom)
    {
        copyRoom.L = L;
        copyRoom.R = R;
        copyRoom.D = D;
        copyRoom.T = T;
        copyRoom.start = start;
        copyRoom.deadEnd = deadEnd;
        copyRoom.bigger = bigger;
        copyRoom.used = used;
        copyRoom.roomSize = roomSize;
    }
    public string getDoors()
    {
        string aux = "";
        return aux = L.ToString() + R.ToString() + D.ToString() + T.ToString();
    }

    public bool doorPassing()
    {
        visited = true;
        inRoom = !inRoom;
        return inRoom;
    }
}
