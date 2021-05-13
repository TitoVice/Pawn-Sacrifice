using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGetHit : EnemyGetHit
{
    public GameObject WinMenu;
    private DeathWinMenu winScript;

    public override void Death()
    {
        //Pre: ---
        //Post: kills the boss

        WinMenu = GameObject.Find("DeathWinMenu");
        winScript = WinMenu.GetComponent<DeathWinMenu>();

        GetComponent<EnemyMoveScript>().Immobilize();
        GetComponent<BoxCollider2D>().enabled = false;
        animator.SetBool("dead", true); 
    }

    public void Win()
    {
        //Pre: ---
        //Post: win state of the game
        
        WinMenu.SetActive(true);
        winScript.Win();
    }
}
