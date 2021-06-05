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
    }

    public void CloseDoor()
    {
        animator.SetBool("closing", true);
        colid.isTrigger = false;
    }

    public void Closed()
    {
        animator.SetBool("closing", false);
        animator.SetBool("closed", true);
    }

    public void TeamInRoom(GameObject team)
    {
        int numEnemies = 0;
        foreach (Transform child in enemies.transform)
        {
            numEnemies++;
            child.GetComponent<EnemyWaiting>().stopWaiting();
            if (child.GetComponent<EnemyMoveScript>()) { child.GetComponent<EnemyMoveScript>().getCharacters(team); }
            else if (child.GetComponent<ChargerMovementScript>()) { child.GetComponent<ChargerMovementScript>().getCharacters(team); }
        }
        if (numEnemies > 0)
        {
            foreach (Transform child in transform.parent)
            {
                child.GetComponent<InRoomBehaviour>().CloseDoor();
            }
        }
        
        room.getTeam(team);
        room.enterRoom();
    }
}
