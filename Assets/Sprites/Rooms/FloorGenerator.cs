using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public GameObject basicRoom;
    public GameObject roomTemplates;
    public GameObject start;
    public GameObject end;
    public TeamWorldInteraction team;

    private RoomTemplates rooms;

    private RoomDoors[,] preRoomGrid;
    private GameObject[,] roomGrid;

    private int[] initialRoom = { 2, 2 };
    private int[] finalRoom = { 0, 0 };
    private List <RoomDoors> possibleEndRooms;

    private int maxRooms = 13;
    private int minRooms = 8;
    private int minDeadEnds = 1;
    private int generatedRooms;
    private int level = 1;
    private bool ready;

    [SerializeField]
    private int size = 10; //size of the rooms that multiplies with the position in the grid
    void Start()
    {
        Create();
    }

    private bool generate(int x, int y, int[] prevPos)
    {
        //Pre: position x and y of the grid
        //Post: generates a room in the grid and returns if a room is generated

        int[] pos = { x, y };
        
        if (generatedRooms == 0)
        {//generates the initial room
            
            preRoomGrid[x, y].getPositionSpecial(pos, true, false, prevPos);
            generatedRooms += 1;

            int randomNumber = Random.Range(0, 4);

            switch (randomNumber)//only one door for the starting room
            {
                case 0:
                    if(generate(x - 1, y, pos)) 
                    {
                        int[] aux = { x - 1, y };
                        preRoomGrid[x, y].openDoor(aux); 
                    }
                    break;
                case 1:
                    if (generate(x + 1, y, pos))
                    {
                        int[] aux = { x + 1, y };
                        preRoomGrid[x, y].openDoor(aux);
                    }
                    break;
                case 2:
                    if (generate(x, y - 1, pos))
                    {
                        int[] aux = { x, y - 1 };
                        preRoomGrid[x, y].openDoor(aux);
                    }
                    break;
                case 3:
                    if (generate(x, y + 1, pos))
                    {
                        int[] aux = { x, y + 1 };
                        preRoomGrid[x, y].openDoor(aux);
                    }
                    break;
            }

            return true;
        }
        else if(generatedRooms < maxRooms)
        {
            if (!isInside(x, y)) { return false; } // off limits 
            else if (preRoomGrid[x, y].used) { return false; } //already there's a room
            else
            {
                int random = Random.Range(0, 4);
                if (random != 0) //if 0 don't generate room
                {
                    random = Random.Range(0, 5);
                    if (random != 0)//if 0 generate dead end room
                    {
                        generation(x, y, preRoomGrid[x, y], prevPos); 
                        return true;
                    }
                    else
                    {
                        preRoomGrid[x, y].getPositionSpecial(pos, false, true, prevPos);
                        preRoomGrid[x, y].openDoor(prevPos);
                        generatedRooms += 1;
                        if (preRoomGrid[x, y].deadEnd) { possibleEndRooms.Add(preRoomGrid[x, y]); }

                        return true;
                    }
                }
                else { return false; }
            }
        }
        else { return false; }
    }

    private void generation(int x, int y, RoomDoors preRoom, int[] prevPos)
    {
        //Pre: valid position of the grid
        //Post: room in the grid, and neighbours generation

        int[] pos = { x, y };

        preRoom.getPositionSpecial(pos, false, false, prevPos);
        preRoom.openDoor(prevPos);
        generatedRooms += 1;

        if(generate(x - 1, y, pos))
        {
            int[] aux = { x - 1, y };
            preRoom.openDoor(aux);
        }
        if (generate(x + 1, y, pos))
        {
            int[] aux = { x + 1, y };
            preRoom.openDoor(aux);
        }
        if (generate(x, y - 1, pos))
        {
            int[] aux = { x, y - 1 };
            preRoom.openDoor(aux);
        }
        if (generate(x, y + 1, pos))
        {
            int[] aux = { x, y + 1 };
            preRoom.openDoor(aux);
        }
    }

    private void initializeGrid()
    {
        //Pre: --
        //Post: initialize the grid of nulls

        for (int i = 0; i < roomGrid.GetLength(0); i++)
        {
            for (int j = 0; j < roomGrid.GetLength(1); j++)
            {
                possibleEndRooms = new List<RoomDoors>();
                roomGrid[i, j] = null;
                GameObject auxRoom = new GameObject("auxRoom", typeof(RoomDoors));
                auxRoom.transform.parent = transform;
                preRoomGrid[i, j] = auxRoom.GetComponent<RoomDoors>();
            }
        }
    }

    private void instantiateRooms()
    {
        //Pre: grid with rooms
        //Post: rooms instantiated in the game

        for (int i = 0; i < roomGrid.GetLength(0); i++)
        {
            for (int j = 0; j < roomGrid.GetLength(1); j++)
            {
                if (preRoomGrid[i, j].used)
                {
                    string doorsNeeded = ""; //format: XXXX -> LRDT, 0 if it does not need that door, 1 if it does
                    roomRevision(i, j);
                    doorsNeeded = preRoomGrid[i, j].getDoors();

                    GameObject auxRoom = findAndCreateRoom(doorsNeeded, i, j);
                    preRoomGrid[i, j].getValues(auxRoom.GetComponent<RoomDoors>());
                    
                    roomGrid[i, j] = auxRoom;
                }
            }
        }
    }
    
    private void roomRevision(int x, int y)
    {
        int[] pos = { x, y };

        if (!preRoomGrid[x, y].start && !preRoomGrid[x, y].deadEnd)//it's not the start and it's not a dead end
        {
            if (isInside(x - 1, y)) { openDoor(preRoomGrid[x, y], preRoomGrid[x - 1, y], x - 1, y, pos); }//left
            if (isInside(x + 1, y)) { openDoor(preRoomGrid[x, y], preRoomGrid[x + 1, y], x + 1, y, pos); }//right
            if (isInside(x, y - 1)) { openDoor(preRoomGrid[x, y], preRoomGrid[x, y - 1], x, y - 1, pos); }//down
            if (isInside(x, y + 1)) { openDoor(preRoomGrid[x, y], preRoomGrid[x, y + 1], x, y + 1, pos); }//top
        }
    }

    private void openDoor(RoomDoors actualRoom, RoomDoors sideRoom, int sideX, int sideY, int[] pos)
    {
        if (sideRoom.used && !sideRoom.start && !sideRoom.deadEnd)//there's a room but it's not the start or a dead end room
        {
            int probability = Random.Range(0, 3);
            if (probability == 0)
            {
                int[] sidePos = { sideX, sideY };
                actualRoom.openDoor(sidePos);
                sideRoom.openDoor(pos);

                if (roomGrid[sideX, sideY] != null)
                {
                    Destroy(roomGrid[sideX, sideY]);
                
                    GameObject auxRoom = findAndCreateRoom(sideRoom.getDoors(), sideX, sideY);
                    sideRoom.getValues(auxRoom.GetComponent<RoomDoors>());

                    roomGrid[sideX, sideY] = auxRoom;
                }
            }
        }
    }

    private GameObject findAndCreateRoom(string doorsNeeded,int x, int y)
    {
        //Pre: string of doors needed by the room,position x and y in the grid
        //Post: has found the adequate room and instantiated it in the game

        for (int i = 0; i < rooms.setOfRooms.Length; i++)
        {
            string doors = rooms.setOfRooms[i].GetComponent<RoomDoors>().doors;
            if (string.Equals(doors, doorsNeeded))
            {
                return Instantiate(rooms.setOfRooms[i], new Vector3(x * size, y * size, 0), Quaternion.identity, transform);
            }
        }
        return null;
    }
    private bool isInside(int x, int y)
    {
        //Pre: x and y are a position
        //Post: true if its inside the grid, false if don't

        if (x < 0 || y < 0 || x >= roomGrid.GetLength(0) || y >= roomGrid.GetLength(1))
        {
            return false;
        }
        else { return true; }
    }

    public void Clear()
    {
        generatedRooms = 0;

        int children = transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            Destroy(transform.GetChild(i).gameObject);//use (0) if its not in execution
        }
    }

    public void Create()
    {
        Clear();
        roomGrid = new GameObject[5, 5];
        preRoomGrid = new RoomDoors[5, 5];
        generatedRooms = 0;
        ready = false;

        rooms = roomTemplates.GetComponent<RoomTemplates>();

        while (!ready)
        {
            initializeGrid();
            generatedRooms = 0;

            generate(initialRoom[0], initialRoom[1], initialRoom);

            if (generatedRooms >= minRooms && generatedRooms <= maxRooms && minDeadEnds <= possibleEndRooms.Count)
            {
                ready = true;
            }
        }

        instantiateRooms();

        int endPos = Random.Range(0, possibleEndRooms.Count);
        RoomDoors endRoom = possibleEndRooms[endPos];            //l'haure d'utilitzar en un futur
        finalRoom[0] = endRoom.position[0];
        finalRoom[1] = endRoom.position[1];

        Instantiate(start, new Vector3(initialRoom[0] * size, initialRoom[1] * size, 0), Quaternion.identity, transform);
        Instantiate(end, new Vector3(finalRoom[0] * size, finalRoom[1] * size, 0), Quaternion.identity, transform);

        team.spawn(initialRoom[0] * size, initialRoom[1] * size);

        level += 1;
        initialRoom[0] = finalRoom[0];
        initialRoom[1] = finalRoom[1];
    }
}
