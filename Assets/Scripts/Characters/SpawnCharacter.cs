using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCharacter : MonoBehaviour
{
    public void spawn(int[] spawn)
    {
        transform.position = new Vector3(spawn[0], spawn[1], 0);
    }
}