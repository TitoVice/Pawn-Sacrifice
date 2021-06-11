using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterDeath : MonoBehaviour
{
    private bool isPlayer;
    private GameObject winLoseMenu;
    public Animator animator;
    public SpriteRenderer sprite;
    public NavMeshAgent navMeshAgent;
    public AgentScript agentScript;
    public Rigidbody2D rb2;
    public BoxCollider2D boxCollider2D;
    private CharacterGetHit getHit;
    public bool isDead = false;
    private int roomsPassed = 0;

    void Start()
    {
        isPlayer = gameObject.CompareTag("Player");

        foreach (Transform child in transform)
        {
            if (child.CompareTag("HitDetector")) { getHit = child.GetComponent<CharacterGetHit>(); }
        }
    }

    public void Death()
    {
        bool revived = false;

        foreach (Transform child in transform.parent)
        {
            if (child.GetComponent<ReviveBehaviour>())
            {
                ReviveBehaviour  revive = child.GetComponent<ReviveBehaviour>();

                if (!revive.usedInFloor) //can be revived
                {
                    Revived();
                    revive.usedInFloor = true;
                    revived = true;
                    StartCoroutine("Flash");
                }
                break;
            }
        }
        if (!revived) { DeathTransition(); }
    }

    private void DeathTransition()
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
            agentScript.enabled = false;
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
            if (roomsPassed >= 4) { roomsPassed = 0; Revived(); }
        }
    }

    public void RestartCounter()
    {
        roomsPassed = 0;
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
            agentScript.enabled = true;
            rb2.constraints = RigidbodyConstraints2D.None;
        }

        getHit.CureHealth();
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

    public void getMenu(GameObject menu)
    {
        winLoseMenu = menu;
    }

    IEnumerator Flash()
    {
        //coroutine to flash the ally as it was revived by an ability
        for (int i = 0; i < 3; i++){
            sprite.color = Color.green;
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);  
        }
    }
}
