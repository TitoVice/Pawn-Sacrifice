using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public void focusRoom(Vector3 pos)
    {
        //Pre: ----
        //Post: move the camera

        transform.position = new Vector3((pos.x/10f)*0.5f -10, (pos.y/10f)*0.5f -10, transform.position.z);
    }
}
