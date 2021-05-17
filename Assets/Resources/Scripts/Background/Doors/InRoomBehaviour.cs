using UnityEngine;

public class InRoomBehaviour : MonoBehaviour
{
    public GameObject enemies;
    public Animator animator;
    public BoxCollider2D colid;
    private RoomBehavior room;
    private bool roomFinished = false;

    void Start()
    {
        room = transform.parent.parent.GetComponent<RoomBehavior>();
    }

    public void OpenDoor()
    {
        animator.SetBool("opening", true);
        colid.isTrigger = true;
        print("Door Opened");
    }

    public void CloseDoor()
    {
        animator.SetBool("closing", true);
        colid.isTrigger = false;
        print("Closing Door");
    }

    public void Closed()
    {
        animator.SetBool("closing", false);
        animator.SetBool("closed", true);
        print("Door Closed");
    }

    public void TeamInRoom(GameObject team)
    {
        CloseDoor();
        foreach (Transform child in enemies.transform)
        {
            child.GetComponent<EnemyWaiting>().stopWaiting();
            if (child.GetComponent<EnemyMoveScript>() != null) { child.GetComponent<EnemyMoveScript>().getCharacters(team); }
            else if (child.GetComponent<ChargerMovementScript>() != null) { child.GetComponent<ChargerMovementScript>().getCharacters(team); }
        }
        foreach (Transform child in transform.parent)
        {
            child.GetComponent<InRoomBehaviour>().CloseDoor();
        }
        room.getTeam(team);
        room.enterRoom();
    }
}