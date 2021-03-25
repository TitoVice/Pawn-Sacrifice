using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerRoomCapacity : MonoBehaviour
{

    public Vector2[] LTentrance = {};
    public Vector2[] RDentrance = {};
    public string name = "";


    public Vector2[] getCapacity(char side)
    {
        Vector2[] solution;

        if (side == 'L' || side == 'T')
        {
            return LTentrance;
        }

        else if (side == 'R' || side == 'D')
        {
            return RDentrance;
        }
        
        return null;
    }

}
