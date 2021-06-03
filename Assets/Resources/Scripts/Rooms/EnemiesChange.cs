using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesChange : MonoBehaviour
{
    private RoomBehavior roomBehavior;
    private int enemies;
    void Start()
    {
        roomBehavior = transform.parent.GetComponent<RoomBehavior>();
        enemies = transform.childCount;
    }

    void Update()
    {
        if (transform.childCount != enemies) 
        {
            enemies = transform.childCount;
            roomBehavior.EnemyListChange();
        }
    }
}
