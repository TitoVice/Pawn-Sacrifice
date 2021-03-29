using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsInteraction : MonoBehaviour
{
    private GameObject camera;
    private RoomDoors parent;
    private PlayerMovement player = null;
    private CameraFreeMovement cameraScript;
    private bool isVertical;

    private bool hasToMove = false;
    private bool freeCamera = false;
    private float rateMoving = 1.0f/0.5f;
    private float t = 0f;

    private Vector3 startPos;//camera positions
    private Vector3 endPos;

    private Vector2 enterPosition;//player positions
    private Vector2 exitPosition;
    private float offset = 0.4f;

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraScript = camera.GetComponent<CameraFreeMovement>();
        parent = transform.parent.transform.parent.GetComponent<RoomDoors>();

        if (gameObject.name[0] == 'D' || gameObject.name[0] == 'T')
        {
            isVertical = false;
        }
        else if (gameObject.name[0] == 'L' || gameObject.name[0] == 'R')
        {
            isVertical = true;
        }
    }

    void FixedUpdate()
    {
        if (hasToMove && t <= 1)
        {
            t += Time.fixedDeltaTime * rateMoving;
            camera.transform.position = Vector3.Lerp(startPos, endPos, t);
            //camera.transform.position = Vector3.Lerp(camera.transform.position, endPos, Time.fixedDeltaTime * rateMoving);
        }
        else if (t >= 1.0f) 
        { 
            hasToMove = false; 
            t = 0; 
            player.Mobilize(); 
            movingCamera();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player == null) { player = collision.gameObject.GetComponent<PlayerMovement>(); }

            parent.enterRoom();

            enterPosition.x = collision.gameObject.transform.position.x;
            enterPosition.y = collision.gameObject.transform.position.y;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            exitPosition.x = collision.gameObject.transform.position.x;
            exitPosition.y = collision.gameObject.transform.position.y;
            
            if (isVertical)
            {
                if (Mathf.Abs(exitPosition.x - enterPosition.x) > offset)//change of room
                {
                    if (parent.leaveRoom()){ moveCamera(collision); }
                }
            }
            else
            {
                if (Mathf.Abs(exitPosition.y - enterPosition.y) > offset)//change of room
                {
                    if (parent.leaveRoom()){ moveCamera(collision); }
                }
            }
        }
    }

    private void moveCamera(Collider2D objective)
    {
        startPos = camera.transform.position;
        player.Immobilize();
        hasToMove = true;
        freeCamera = false;

        if (parent.bigger)//the camera follows the player
        {
            endPos = new Vector3(objective.gameObject.transform.position.x, objective.gameObject.transform.position.y, camera.transform.position.z);
            freeCamera = true;
        }
        else //the camera is still
        { 
            endPos = new Vector3(parent.transform.position.x, parent.transform.position.y, camera.transform.position.z);
        }
    }

    private void movingCamera()
    {
        if (freeCamera) { cameraScript.startFreeMove(); }
        else { cameraScript.stopFreeMove(); }
    }

}
