using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallColisions : MonoBehaviour
{

    public List<int> walls;
    /*  
        1: have down wall
        2: have top wall
        3: have left wall
        4: have right wall
    */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WallPoint"))
        {
            for (int i = 0; i < collision.GetComponent<WallColisions>().walls.Count; i++)
            {

            }
           // walls.Add(collision.GetComponent<WallColisions>().walls);
        }
    }
}
