using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapDisplayer : MonoBehaviour
{
    private class miniRoom{
        public string doors; //XXXX == LRDT, If 1 it has a door in that side
        public bool visited = false;
        public  GameObject roomSprite = null;
        public List<Vector2> bigRoomsNexts = new List<Vector2>();
    }

    private miniRoom[,] miniMapGrid = new miniRoom[9, 9];
    private miniRoom endRoom;
    private Vector2 actualGridPos;
    private float roomSize = 0.5f;
    public GameObject[] roomSprites;
    public GameObject team;

    public void getNormalRoom(RoomDoors room, int[] pos)
    {
        miniRoom auxRoom = new miniRoom();
        
        foreach (GameObject sprite in roomSprites)
        {
            if (!room.end)
            {
                if (sprite.name == "basicRoom")
                {
                    auxRoom.roomSprite = sprite;
                    auxRoom.visited = room.visited;
                    auxRoom.roomSprite = Instantiate(auxRoom.roomSprite, new Vector3((pos[0] * roomSize)-10, (pos[1] * roomSize)-10, 0), Quaternion.identity, transform);
                    miniMapGrid[pos[0], pos[1]] = auxRoom;
                }
            }
            else
            {
                if (sprite.name == "EndRoom")
                {
                    auxRoom.roomSprite = sprite;
                    Destroy(miniMapGrid[pos[0], pos[1]].roomSprite);
                    auxRoom.roomSprite = Instantiate(auxRoom.roomSprite, new Vector3((pos[0] * roomSize)-10, (pos[1] * roomSize)-10, 0), Quaternion.identity, transform);
                    miniMapGrid[pos[0], pos[1]] = auxRoom;
                    endRoom = auxRoom;
                }
            }
            
        }
    }

    public void getBigRoom(BiggerRoomCapacity room, int[] pos, int[] center)
    {
        miniRoom auxRoom = new miniRoom();

        foreach (GameObject sprite in roomSprites)
        {
            if (sprite.name == room.roomName)
            {
                auxRoom.roomSprite = sprite;

                Vector2[] auxGrids = room.getCapacity(room.entranceSide);
                foreach (Vector2 grid in auxGrids) { auxRoom.bigRoomsNexts.Add(grid); }

                auxRoom.roomSprite = Instantiate(auxRoom.roomSprite, new Vector3(((pos[0] * roomSize) + ( (center[0]/10f)/2f ))-10f, ((pos[1] * roomSize) + ((center[1]/10f)/2f))-10f, 0), Quaternion.identity, transform);
                miniMapGrid[pos[0], pos[1]] = auxRoom;
            }
        }
    }

    public void roomToShow(int[] pos)
    {
        if (miniMapGrid[pos[0], pos[1]] != null) { show(miniMapGrid[pos[0], pos[1]], pos); }
    }

    public void getDoors(int[] pos, string doors)
    {
        if (miniMapGrid[pos[0], pos[1]] != null) { miniMapGrid[pos[0], pos[1]].doors = doors; }
    }

    private void show(miniRoom room, int[] pos)
    {
        showRoom(room);

        if (room.bigRoomsNexts.Count == 0) //normal size rooms
        {
            if (room.doors[0] == '1') 
            {
                if (isInside(pos[0]-1, pos[1]) && miniMapGrid[pos[0]-1, pos[1]] != null)  
                    { 
                        nextToShown(miniMapGrid[pos[0]-1, pos[1]]);
                    }
            }
            if (room.doors[1] == '1') 
            {
                if (isInside(pos[0]+1, pos[1]) && miniMapGrid[pos[0]+1, pos[1]] != null)  
                    { 
                        nextToShown(miniMapGrid[pos[0]+1, pos[1]]);
                    }
            }
            if (room.doors[2] == '1') 
            {
                if (isInside(pos[0], pos[1]-1) && miniMapGrid[pos[0], pos[1]-1] != null)  
                    { 
                        nextToShown(miniMapGrid[pos[0], pos[1]-1]);
                    }
            }
            if (room.doors[3] == '1') 
            {
                if (isInside(pos[0], pos[1]+1) && miniMapGrid[pos[0], pos[1]+1] != null)  
                    { 
                        nextToShown(miniMapGrid[pos[0], pos[1]+1]);
                    }
            }
        }
        else //big rooms
        {
            foreach (Vector2 position in room.bigRoomsNexts)
            {
                int[] auxPos = { pos[0]+(int)position.x, pos[1]+(int)position.y };

                if (isInside(auxPos[0], auxPos[1]) && miniMapGrid[auxPos[0], auxPos[1]] != null)
                {
                    nextToShown(miniMapGrid[auxPos[0], auxPos[1]]);
                }
            }
        }
    }

    private void showRoom(miniRoom room)
    {
        SpriteRenderer sprite = room.roomSprite.transform.GetChild(0).GetComponent<SpriteRenderer>();
        sprite.enabled = true;
        sprite.color = Color.white;
        room.visited = true;
    }

    public void showEndRoom()
    {
        //Pre: ---
        //Post: shows the end room if vision is active

        showRoom(endRoom);
    }

    private void nextToShown(miniRoom room)
    {
        if (!room.visited)
        {
            SpriteRenderer sprite = room.roomSprite.transform.GetChild(0).GetComponent<SpriteRenderer>();
            sprite.enabled = true;
            sprite.color = Color.grey;
        }
        else { showRoom(room); } //for the first room
    }

    public void Clear()
    {
        //Pre: ---
        //Post: all children deleted

        foreach (Transform child in transform)
        {
            if (child.name != "camera") { Destroy(child.gameObject); }
        }
        
        miniMapGrid = new miniRoom[9, 9];
    }

    private bool isInside(int x, int y)
    {
        //Pre: x and y are a position
        //Post: true if its inside the grid, false if don't

        if (x < 0 || y < 0 || x >= miniMapGrid.GetLength(0) || y >= miniMapGrid.GetLength(1))
        {
            return false;
        }
        else { return true; }
    }
}
