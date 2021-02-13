using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField]
    private int openingDirection;
    /*  
        1: need down room
        2: need top room
        3: need left room
        4: need right room
    */

    private RoomTemplates templates;
    private bool spawned = false;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.05f);
    }

    // Quaternion.Identity
    void Spawn()
    {
        if (!spawned)
        {
            switch (openingDirection)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
            spawned = true;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("SpawnPoint"))
        {

        }
    }

}
