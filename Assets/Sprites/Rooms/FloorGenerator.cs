using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public GameObject basicRoom;
    public GameObject roomTemplates;
    
    private RoomTemplates rooms;

    private GameObject[,] roomGrid;

    private int[] initialRoom = { 2, 2 };
    private int[] finalRoom = { 0, 0 };

    private int maxRooms = 13;
    private int minRooms = 8;
    private int generatedRooms;
    private bool ready;

    [SerializeField]
    private int size = 10; //size of the rooms that multiplies with the position in the grid
    void Start()
    {
        Create();
    }

    private void generate(int x, int y)
    {
        //Pre: position x and y of the grid
        //Post: generates a room in the grid

        if (generatedRooms == 0)
        {//generates the initial room
            generation(x, y);
        }
        else if(generatedRooms < maxRooms)
        {
            if (!isInside(x, y)) { } // off limits 
            else if (roomGrid[x, y] != null) { } //already there's a room
            else
            {
                int random = Random.Range(0, 4);
                if (random != 0) //if 0 don't generate room
                {
                    random = Random.Range(0, 5);
                    if (random != 0)//if 0 generate dead end room
                    {
                        generation(x, y);
                    }
                    else
                    {
                        roomGrid[x, y] = basicRoom;
                        generatedRooms++;
                    }
                }
            }
        }

    }

    private void generation(int x, int y)
    {
        //Pre: valid position of the grid
        //Post: room in the grid, and neighbours generation

        roomGrid[x, y] = basicRoom;
        generatedRooms++;

        generate(x - 1, y);
        generate(x + 1, y);
        generate(x, y - 1);
        generate(x, y + 1);
    }

    private void initializeGrid()
    {
        //Pre: --
        //Post: initialize the grid of nulls

        for (int i = 0; i < roomGrid.GetLength(0); i++)
        {
            for (int j = 0; j < roomGrid.GetLength(1); j++)
            {
                roomGrid[i, j] = null;
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
                if (roomGrid[i, j] != null)
                {
                    int[] doorsNeeded = { -1, -1, -1, -1 }; //0: need left door 1: need down door 2: need right door 3: need top door
                    int doors = 0;
                    for (int x = -1; x < 2; x+=2)//left and right rooms
                    {
                        if (isInside(i + x, j))//not off limits
                        {
                            if (roomGrid[i + x, j] != null)//theres a room
                            {
                                doorsNeeded[doors] = x + 1; //sums 1 to get 0=left, 2=right
                                doors += 1;
                            }
                        }
                        
                    }
                    for (int y = -1; y < 2; y += 2)//top and down rooms
                    {
                        if (isInside(i, j + y))//not off limits
                        {
                            if (roomGrid[i, j + y] != null)//theres a room
                            {
                                doorsNeeded[doors] = y + 2; //sums 1 to get 1=down, 3=top
                                doors += 1;
                            }
                        }

                    }

                    switch (doors)
                    {
                        case 1:
                            roomGrid[i, j] = findAndCreateRoom(rooms.oneDoor, doorsNeeded, doors, i, j);
                            break;
                        case 2:
                            roomGrid[i, j] = findAndCreateRoom(rooms.twoDoors, doorsNeeded, doors, i, j);
                            break;
                        case 3:
                            roomGrid[i, j] = findAndCreateRoom(rooms.threeDoors, doorsNeeded, doors, i, j);
                            break;
                        case 4:
                            roomGrid[i, j] = findAndCreateRoom(rooms.fourDoors, doorsNeeded, doors, i, j);
                            break;
                    }

                    //aqui es on crees l'habitacio, busques a la llista que tingui numPortes==doors, dintre busques la que tingui les portes necessaries.
                    //instancies gracies a la variable size i la pos a la matriu

                }
            }
        }
    }

    /// <summary>
    /// fa coso
    /// </summary>
    /// <param name="setOfRooms"></param>
    /// <param name="doorsNeeded"></param>
    /// <param name="doors"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private GameObject findAndCreateRoom(GameObject[] setOfRooms, int[] doorsNeeded, int doors, int x, int y)
    {
        //Pre: array of doors needed by the room, quantity of doors needed, position x and y in the grid
        //Post: has found the adequate room and instantiated it in the game

        bool finish = false;
        int z = 0;

        while (!finish && z < setOfRooms.Length)
        {
            int[] auxDoors = setOfRooms[z].GetComponent<RoomDoors>().doors;
            int p = 0;
            bool notThis = false;
            while (p < doors && !notThis)
            {
                int n = 0;
                bool found = false;
                while (n < auxDoors.Length && !found)
                {
                    if (doorsNeeded[p] == auxDoors[n])
                    {
                        found = true;
                    }
                    n++;
                }
                if (!found)
                {
                    notThis = true;
                }
                p++;
            }
            if (!notThis)//room founded
            {
                finish = true;
            }
            else { z++; }
        }

        Instantiate(setOfRooms[z], new Vector3(x * size, y * size, 0), Quaternion.identity, transform);
        return setOfRooms[z];
    }
    private bool isInside(int x, int y)
    {
        //Pre: x and y are a position
        //Post: true if its inside the grid, false if don't

        if (x < 0 || y < 0 || x >= roomGrid.GetLength(0) || y >= roomGrid.GetLength(1))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Clear()
    {
        initializeGrid();
        generatedRooms = 0;

        int children = transform.childCount;
        for (int i = 0; i < children; ++i)
            DestroyImmediate(transform.GetChild(0).gameObject);
    }

    public void Create()
    {
        roomGrid = new GameObject[5, 5];
        generatedRooms = 0;
        ready = false;

        rooms = roomTemplates.GetComponent<RoomTemplates>();

        while (!ready)
        {
            initializeGrid();
            generatedRooms = 0;

            generate(initialRoom[0], initialRoom[1]);
            if (generatedRooms >= minRooms && generatedRooms <= maxRooms)
            {
                ready = true;
            }
        }

        instantiateRooms();
    }

}
