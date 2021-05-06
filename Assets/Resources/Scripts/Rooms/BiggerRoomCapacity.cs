using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerRoomCapacity : MonoBehaviour
{

    public Vector2[] LTentrance = {};
    public Vector2[] RDentrance = {};
    public string roomName = "";
    public char entranceSide = ' ';
    public int[] LTentranceCell;
    public int[] RDentranceCell;


    public Vector2[] getCapacity(char side)
    {
        entranceSide = side;
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

    public int[] getCentralPoint(char side)
    {
        entranceSide = side;
        if (side == 'L' || side == 'T')
        {
            return LTentranceCell;
        }

        else if (side == 'R' || side == 'D')
        {
            return RDentranceCell;
        }
        
        return null;
    }

}
