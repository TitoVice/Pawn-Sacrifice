using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour
{
    public SpriteRenderer sprite;
    public CircleCollider2D collid;
    public bool active = true;

    private int neededRooms = 3;
    private int actualRooms = 0;

    public void enterRoom()
    {
        if (!active)
        {
            actualRooms += 1;
            if (actualRooms >= neededRooms) { Activate(); }
        }
    }

    public void Desactivate()
    {
        sprite.enabled = false;
        collid.enabled = false;
        active = false;
    }

    public void Activate()
    {
        sprite.enabled = true;
        collid.enabled = true;
        active = true;
        actualRooms = 0; 
    }
}
