using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapDisplayer : MonoBehaviour
{
    private class miniRoom{
        public string doors; //XXXX == LRDT, If 1 it has a door in that side
        public bool visited = false;
        public bool nextToVisited = false;
        public  GameObject roomSprite = null;
        public Vector2[] bigRoomsNexts = {new Vector2(-1,-1), new Vector2(-1,-1)};
    }

    private miniRoom[,] miniMapGrid;
    private Vector2 actualGridPos;
    private float roomSize = 0.25f;
    public GameObject[] roomSprites;
    void Start()
    {
        
    }

    public void getNormalRoom(RoomDoors room, int[] pos)
    {
        miniRoom auxRoom = new miniRoom();
        auxRoom.doors = room.doors;
        //posar sprite basic, donar-li posicio i instanciar-lo
        
        foreach (GameObject sprite in roomSprites)
        {
            if (sprite.name == "basicRoom")
            {
                auxRoom.roomSprite = sprite;
                auxRoom.roomSprite.transform.position = new Vector3((pos[0] * roomSize)-11.25f, (pos[1] * roomSize)-11.25f, 0); //-11.25 cause it's the center of the camera minus 5*0.25, 5 cause the grid is 10x10
                Instantiate(auxRoom.roomSprite, transform);
            }
        }
    }

    public void getBigRoom(BiggerRoomCapacity room, int[] pos)
    {
        miniRoom auxRoom = new miniRoom();

        foreach (GameObject sprite in roomSprites)
        {
            if (sprite.name == room.roomName)
            {
                auxRoom.roomSprite = sprite;
                //auxRoom.roomSprite.transform.position = new Vector3((pos[0] * roomSize)-11.25f, (pos[1] * roomSize)-11.25f, 0); //-11.25 cause it's the center of the camera minus 5*0.25, 5 cause the grid is 10x10
                Instantiate(auxRoom.roomSprite, transform);
            }
        }
    }
}
