using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class FloorGenerator : MonoBehaviour
{
    public GameObject basicRoom;
    public GameObject roomTemplates;
    public GameObject start;
    public GameObject end;
    public TeamWorldInteraction team;
    public MiniMapDisplayer miniMap;

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
            
            preRoomGrid[x, y].getPositionSpecial(pos, true, false, false, size);
            miniMap.getNormalRoom(preRoomGrid[x, y], pos);
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
                    if ((random == 0) && generatedRooms < maxRooms - 1)//try to generate big room, needs 2 rooms  
                    {
                        return generateBigRoom(pos, prevPos);
                    }
                    else if (random == 1)//if 1 generate dead end room
                    {
                        preRoomGrid[x, y].getPositionSpecial(pos, false, true, false, size);
                        preRoomGrid[x, y].openDoor(prevPos);
                        miniMap.getNormalRoom(preRoomGrid[x, y], pos);
                        generatedRooms += 1;
                        if (preRoomGrid[x, y].deadEnd) { possibleEndRooms.Add(preRoomGrid[x, y]); }

                        return true;
                    }
                    else
                    {
                        generation(x, y, preRoomGrid[x, y], prevPos); 
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

        preRoom.getPositionSpecial(pos, false, false, false, size);
        preRoom.openDoor(prevPos);
        miniMap.getNormalRoom(preRoom, pos);
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

        possibleEndRooms = new List<RoomDoors>();
        miniMap.Clear();

        for (int i = 0; i < roomGrid.GetLength(0); i++)
        {
            for (int j = 0; j < roomGrid.GetLength(1); j++)
            {
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
                    if (!preRoomGrid[i, j].bigger)
                    {
                        string doorsNeeded = ""; //format: XXXX -> LRDT, 0 if it does not need that door, 1 if it does
                        roomRevision(i, j);
                        doorsNeeded = preRoomGrid[i, j].getDoors();

                        GameObject auxRoom = findAndCreateRoom(doorsNeeded, i, j);
                        preRoomGrid[i, j].getValues(auxRoom.GetComponent<RoomDoors>());
                        
                        roomGrid[i, j] = auxRoom;
                    }
                    else if (preRoomGrid[i, j].bigToSpawn)//it's a bigger room
                    {
                        GameObject auxRoom = findAndCreateBigRoom(preRoomGrid[i, j].bigName, preRoomGrid[i, j].bigRoomSide, i, j);
                        preRoomGrid[i, j].getValues(auxRoom.GetComponent<RoomDoors>());
                        
                        roomGrid[i, j] = auxRoom;
                    }
                }
            }
        }
    }
    
    private void roomRevision(int x, int y)
    {
        int[] pos = { x, y };

        if (!preRoomGrid[x, y].start && !preRoomGrid[x, y].deadEnd && !preRoomGrid[x, y].bigger)//it's not the start and it's not a dead end and not a big room
        {
            if (isInside(x - 1, y)) { openDoor(preRoomGrid[x, y], preRoomGrid[x - 1, y], x - 1, y, pos); }//left
            if (isInside(x + 1, y)) { openDoor(preRoomGrid[x, y], preRoomGrid[x + 1, y], x + 1, y, pos); }//right
            if (isInside(x, y - 1)) { openDoor(preRoomGrid[x, y], preRoomGrid[x, y - 1], x, y - 1, pos); }//down
            if (isInside(x, y + 1)) { openDoor(preRoomGrid[x, y], preRoomGrid[x, y + 1], x, y + 1, pos); }//top
        }
    }

    private void openDoor(RoomDoors actualRoom, RoomDoors sideRoom, int sideX, int sideY, int[] pos)
    {
        if (sideRoom.used && !sideRoom.start && !sideRoom.deadEnd && !sideRoom.bigger)//there's a room but it's not the start or a dead end room
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

    //big rooms generation
    private bool generateBigRoom(int[] pos, int[] prevPos)
    {
        //Pre: valid position in the grid, has to validate the others
        //Post: if possible generates a big room and return true, else false

        int[] newPrevPos = {0, 0};
        int x = pos[0] - prevPos[0];
        int y = pos[1] - prevPos[1];
        int L = 0, R = 0, D = 0, T = 0;

        if (x == 1) { L = 1; }
        else if(x == -1) { R = 1; }
        else
        {
            if(y == 1) { D = 1; }
            else if(y == -1) { T = 1; }
        }

        if (L == 1)
        {
            for (int i = 0; i < rooms.setOfBigRooms.Length; i++)
            {
                if (rooms.setOfBigRooms[i].GetComponent<RoomDoors>().L == 1)
                {
                    if(validateSize(rooms.setOfBigRooms[i], pos, 'L', ref x, ref y, ref newPrevPos))//it fits
                    {
                        generatedRooms += 1;
                        generation(x, y, preRoomGrid[x, y], newPrevPos);
                        return true;
                    }
                }
            }
        }
        else if (R == 1)
        {
            for (int i = 0; i < rooms.setOfBigRooms.Length; i++)
            {
                if (rooms.setOfBigRooms[i].GetComponent<RoomDoors>().R == 1)
                {
                    if(validateSize(rooms.setOfBigRooms[i], pos, 'R', ref x, ref y, ref newPrevPos))//it fits
                    {
                        generatedRooms += 1;
                        generation(x, y, preRoomGrid[x, y], newPrevPos);
                        return true;
                    }
                }
            }
        }
        else if (D == 1)
        {
            for (int i = 0; i < rooms.setOfBigRooms.Length; i++)
            {
                if (rooms.setOfBigRooms[i].GetComponent<RoomDoors>().D == 1)
                {
                    if(validateSize(rooms.setOfBigRooms[i], pos, 'D', ref x, ref y, ref newPrevPos))//it fits
                    {
                        generatedRooms += 1;
                        generation(x, y, preRoomGrid[x, y], newPrevPos);
                        return true;
                    }
                }
            }
        }
        else if (T == 1)
        {
            for (int i = 0; i < rooms.setOfBigRooms.Length; i++)
            {
                if (rooms.setOfBigRooms[i].GetComponent<RoomDoors>().T == 1)
                {
                    if(validateSize(rooms.setOfBigRooms[i], pos, 'T', ref x, ref y, ref newPrevPos))//it fits
                    {
                        generatedRooms += 1;
                        generation(x, y, preRoomGrid[x, y], newPrevPos);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool validateSize(GameObject bigRoom, int[] pos, char side, ref int x, ref int y, ref int[] prevPos)
    {
        //Pre: ----
        //Post: if the room fits return true and marks the grids where it's needed, if not return false

        bool validated = true;
        Vector2[] neededGrids = bigRoom.GetComponent<BiggerRoomCapacity>().getCapacity(side);

        for (int i = 0; i < neededGrids.Length; i++)//sees if the grids are used o inside the grid
        {
            int posX = pos[0]+(int)neededGrids[i].x;
            int posY = pos[1]+(int)neededGrids[i].y;

            if (isInside(posX, posY))
            {
                if (preRoomGrid[posX, posY].used)
                {
                    validated = false;
                    break;
                }
            }
            else
            {
                validated = false;
                break;
            }
        }

        if(validated)
        {
            for (int i = 0; i < neededGrids.Length; i++)
            {
                int posX = pos[0]+(int)neededGrids[i].x;
                int posY = pos[1]+(int)neededGrids[i].y;
                preRoomGrid[posX, posY].getPositionSpecial(pos, false, false, true, 0);
                if (i == 0) { preRoomGrid[posX, posY].getName_Spawn(bigRoom.GetComponent<BiggerRoomCapacity>().roomName, side); }
            }
            prevPos[0] = (int)neededGrids[neededGrids.Length - 2].x + pos[0];
            prevPos[1] = (int)neededGrids[neededGrids.Length - 2].y + pos[1];

            x = (int)neededGrids[neededGrids.Length - 1].x + pos[0];
            y = (int)neededGrids[neededGrids.Length - 1].y + pos[1];
        }

        return validated;
    }
    
    private GameObject findAndCreateBigRoom(string name, char side, int x, int y)
    {
        //Pre: name of the big room, char that says where's the enrtrance, position x and y in the grid
        //Post: has found the adequate big room and instantiated it in the game

        int[] pos = {x, y};

        for (int i = 0; i < rooms.setOfBigRooms.Length; i++)
        {
            string bigRoomName = rooms.setOfBigRooms[i].GetComponent<BiggerRoomCapacity>().roomName;
            if (string.Equals(name, bigRoomName))
            {
                int[] center = rooms.setOfBigRooms[i].GetComponent<BiggerRoomCapacity>().getCentralPoint(side);
                miniMap.getBigRoom(rooms.setOfBigRooms[i].GetComponent<BiggerRoomCapacity>(), pos, center);
                return Instantiate(rooms.setOfBigRooms[i], new Vector3(x * size + center[0], y * size + center[1], 0), Quaternion.identity, transform);
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
        roomGrid = new GameObject[9, 9];
        preRoomGrid = new RoomDoors[9, 9];
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

        //deletes some rubbish
        foreach(Transform t in transform)
        {
            if (t.name == "auxRoom")
            {
                Destroy(t.gameObject);
            }
        }

        int endPos = Random.Range(0, possibleEndRooms.Count);
        RoomDoors endRoom = possibleEndRooms[endPos];       

        //minimap end room part
        int[] endPosition = {endRoom.position[0], endRoom.position[1]};
        RoomDoors auxEndRoom = roomGrid[endPosition[0], endPosition[1]].GetComponent<RoomDoors>();
        auxEndRoom.isEnd();
        miniMap.getNormalRoom(auxEndRoom, endPosition);
        //---------------------

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
