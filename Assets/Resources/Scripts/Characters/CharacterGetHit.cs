using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGetHit : MonoBehaviour
{
    public int initialLife = 5;
    public int life = 5;
    public CapsuleCollider2D capsColider;
    public GameObject winLoseMenu;
    private CharacterStats stats;
    private SpriteRenderer sprite;

    public bool dead = false;
    public float deadTimer = 0.0f;
    private float deadLapsus = 1.5f;
    private bool damaged = false;
    private float damageTimer = 0.0f;
    private float damageCooldown = 0.6f;

    void Start()
    {
        stats = transform.parent.GetComponent<CharacterStats>();
        sprite = transform.parent.GetComponent<SpriteRenderer>();
        winLoseMenu = GameObject.Find("DeathWinMenu");
    }

    void Update()
    {
        if (dead && gameObject.transform.parent.CompareTag("Player"))
        {
            deadTimer += Time.deltaTime;
            if (deadTimer >= deadLapsus) //time to let the revive ability be used, also to have a pause time for the next game
            {
                
                winLoseMenu.SetActive(true);
                winLoseMenu.GetComponent<DeathWinMenu>().Death();
            }
        }
        else if (dead && gameObject.transform.parent.CompareTag("Companion"))
        {
            deadTimer += Time.deltaTime;
            if (deadTimer >= deadLapsus) //time to let the revive ability be used, also to have a pause time for the next game
            {
                transform.parent.GetComponent<AgentScript>().Immobilize();//----------------------------------------------------  reviure al X habitacions, no permetre que es mogui d'on ha mort (freeze position?), desactivar coliders
            }
        }
        else if (damaged)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageCooldown) { damageTimer = 0.0f; damaged = false; }
        }
    }

    public void getHit()
    {
        //Pre: ---
        //Post: character received damage, if it doesn't have a shield

        if (!damaged)
        {
            if (stats.instantiatedShield != null && stats.instantiatedShield.GetComponent<ShieldBehaviour>().active)
            {
                stats.instantiatedShield.GetComponent<ShieldBehaviour>().Desactivate();
            }
            else
            {
                life -= 1;
                if (life <= 0) { dead = true; /*animacio de mort despres posem dead a true en una funcio realment*/  }
                else { StartCoroutine("Flash"); }
            }
            damaged = true;
        }
    }

    public void CureHealth()
    {
        life = initialLife;
    }

    public void Revived()
    {
        dead = false;
        life = initialLife/2;
        deadTimer = 0.0f;
    }

    IEnumerator Flash()
    {
        //coroutine to flash the enemy as it's hit
        for (int i = 0; i < 3; i++){
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);  
        }
    }

}
