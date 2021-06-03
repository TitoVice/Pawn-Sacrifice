using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGetHit : EnemyGetHit
{
    private GameObject Menu;
    private GameObject WinMenu;

    public override void Death()
    {
        //Pre: ---
        //Post: kills the boss

        Menu = GameObject.Find("UI"); //gets the ui
        foreach (Transform child in Menu.transform)
        {
            if (child.GetComponent<DeathWinMenu>()) { WinMenu = child.gameObject; }
        }

        GetComponent<EnemyMoveScript>().Immobilize();
        GetComponent<BoxCollider2D>().enabled = false;
        foreach (Transform child in transform) { child.gameObject.SetActive(false); }
        animator.SetBool("dead", true); 
    }

    public void Win()
    {
        //Pre: ---
        //Post: win state of the game
        
        WinMenu.SetActive(true);
        WinMenu.GetComponent<DeathWinMenu>().Win();
    }
}
