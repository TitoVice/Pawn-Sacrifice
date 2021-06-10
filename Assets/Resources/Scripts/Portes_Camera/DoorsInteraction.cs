using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsInteraction : MonoBehaviour
{
    private GameObject mainCamera;
    private RoomDoors parent;
    private PlayerMovement player = null;
    private CameraFreeMovement cameraScript;
    private TeamWorldInteraction team;
    private InRoomBehaviour inroomBehaviour;
    public bool isVertical;
    public bool isRight, isTop; //if the value is false, it's the opposite direction

    private bool hasToMove = false;
    private bool freeCamera = false;
    private float rateMoving = 4.0f;
    private float t = 0f;

    private Vector3 startPos;//camera positions
    private Vector3 endPos;

    private Vector2 enterPosition;//player positions
    private Vector2 exitPosition;
    private float offset = 0.4f;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraScript = mainCamera.GetComponent<CameraFreeMovement>();
        parent = transform.parent.parent.GetComponent<RoomDoors>();
        team = GameObject.Find("Team").GetComponent<TeamWorldInteraction>();
        inroomBehaviour = GetComponent<InRoomBehaviour>();
    }

    void FixedUpdate()
    {
        if (hasToMove && t <= 1)
        {
            t += Time.fixedDeltaTime * rateMoving;
            mainCamera.transform.position = Vector3.Lerp(startPos, endPos, t);
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
                    if (parent.leaveRoom()){ moveCamera(collision); inroomBehaviour.TeamInRoom(collision.transform.parent.gameObject); parent.enterRoom();}
                }
            }
            else
            {
                if (Mathf.Abs(exitPosition.y - enterPosition.y) > offset)//change of room
                {
                    if (parent.leaveRoom()){ moveCamera(collision); inroomBehaviour.TeamInRoom(collision.transform.parent.gameObject); parent.enterRoom();}
                }
            }
        }
    }

    private void moveCamera(Collider2D objective)
    {
        startPos = mainCamera.transform.position;
        player.Immobilize();
        hasToMove = true;
        freeCamera = false;

        if (parent.bigger)//the camera follows the player
        {
            endPos = new Vector3(objective.gameObject.transform.position.x, objective.gameObject.transform.position.y, mainCamera.transform.position.z);
            freeCamera = true;
        }
        else //the camera is still
        { 
            endPos = new Vector3(parent.transform.position.x, parent.transform.position.y, mainCamera.transform.position.z);
        }

        if (parent.inRoom)//when enters another room move the partners
        {
            team.roomPass(transform.position, isVertical, isTop, isRight);
        }
    }

    private void movingCamera()
    {
        if (freeCamera) { cameraScript.startFreeMove(); }
        else { cameraScript.stopFreeMove(); }
    }

}
