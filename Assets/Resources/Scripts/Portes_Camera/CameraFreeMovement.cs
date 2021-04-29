using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFreeMovement : MonoBehaviour
{
    private bool hasToMove = false;
    private float rateMoving = 1.5f;
    private Vector3 startPos;//camera positions
    private Vector3 endPos;

    private GameObject parentPlayer = null;

    void Update()
    {
        if (hasToMove)
        {
            Vector3 playerPos = new Vector3(parentPlayer.transform.position.x, parentPlayer.transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, playerPos, Time.deltaTime * rateMoving);
        }
    }

    public void startFreeMove()
    {
        if (parentPlayer == null) { parentPlayer = GameObject.FindGameObjectsWithTag("Player")[0]; }
        hasToMove = true;
    }

    public void stopFreeMove()
    {
        hasToMove = false;
    }
}
