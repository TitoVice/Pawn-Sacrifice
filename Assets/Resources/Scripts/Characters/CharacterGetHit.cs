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
    private CharacterDeath characterDeath;
    private SpriteRenderer sprite;
    private bool damaged = false;
    private float damageTimer = 0.0f;
    private float damageCooldown = 0.6f;

    void Start()
    {
        stats = transform.parent.GetComponent<CharacterStats>();
        sprite = transform.parent.GetComponent<SpriteRenderer>();
        characterDeath = transform.parent.GetComponent<CharacterDeath>();
        winLoseMenu = GameObject.Find("DeathWinMenu");                      //no val aixo
    }

    void Update()
    {
        if (damaged)
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
                if (life <= 0) { characterDeath.Death(); }
                else { StartCoroutine("Flash"); }
            }
            damaged = true;
        }
    }

    public void CureHealth()
    {
        print(transform.name+": "+life);
        life = initialLife;
        print(transform.name+": "+life);
    }

    public void Revived()
    {
        life = initialLife/2;
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
