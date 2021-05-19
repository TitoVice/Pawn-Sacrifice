using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterDeath : MonoBehaviour
{
    private bool isPlayer;
    private GameObject winLoseMenu;
    public Animator animator;
    public NavMeshAgent navMeshAgent;
    public Rigidbody2D rb2;
    public BoxCollider2D boxCollider2D;
    private CharacterGetHit getHit;
    public bool isDead = false;
    private int roomsPassed = 0;

    void Start()
    {
        isPlayer = gameObject.CompareTag("Player");
        winLoseMenu = GameObject.Find("DeathWinMenu");

        foreach (Transform child in transform)
        {
            if (child.CompareTag("HitDetector")) { getHit = child.GetComponent<CharacterGetHit>(); }
        }
    }

    public void Death()
    {
        animator.SetBool("dead", true);
        isDead = true;
        SetActivity(false);

        if (isPlayer)
        {
            GetComponent<PlayerMovement>().Immobilize();
        }
        else
        {
            boxCollider2D.enabled = false;
            navMeshAgent.enabled = false;
            rb2.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }
    }

    public void StillDead()
    {
        //Pre: after death animation
        //Post: if it's player end game, else left the body dead without colliders

        if (isPlayer)
        {
            winLoseMenu.SetActive(true);
            winLoseMenu.GetComponent<DeathWinMenu>().Death();
        }
    }

    public void PassRoom()
    {
        //Pre: ---
        //Post: if rooms passed is >= 3, the comapnion revives

        if (isDead)
        {
            roomsPassed++;
            if (roomsPassed >= 5) { roomsPassed = 0; Revived(); }
        }
    }

    public void Revived()
    {
        //Pre: ---
        //Post: revive

        animator.SetBool("dead", false);
        isDead = false;
        SetActivity(true);

        if (isPlayer)
        {
            GetComponent<PlayerMovement>().Mobilize();
        }
        else
        {
            boxCollider2D.enabled = true;
            navMeshAgent.enabled = true;
            rb2.constraints = RigidbodyConstraints2D.None;
        }

        getHit.Revived();
    }

    private void SetActivity(bool what)
    {
        //Pre: set true or false
        //Post: sets the childs to what

        foreach(Transform child in transform)
            {
                child.gameObject.SetActive(what);
            }
    }
}
