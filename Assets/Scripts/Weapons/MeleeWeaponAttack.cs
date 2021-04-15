using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponAttack : MonoBehaviour
{
    
    private Dictionary<string, bool> stats;
    private WeaponAttack weapon;
    private float meleeDamage;
    private bool noEffects = true;

    private float timer = 0.0f;
    private float coolDown = 1.0f;
    private float retard = 0.06f;
    private bool colided = false;

    void Start()
    {
        weapon = GetComponent<WeaponAttack>();
    }

    void Update()
    {
        if (colided)
        {
            timer += Time.deltaTime;
            if (timer >= retard) 
            { 
                weapon.returnMeleeWeapon(); 
                timer = 0.0f; 
                colided = false; 
            }
        }
    }

    public void getVariables(Dictionary<string, bool> weaponStats, float damage)
    {
        //Pre: ---
        //Post: getter

        stats = weaponStats;
        meleeDamage = damage;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Pre:---
        //Post: hits the enemy with a melee attack

        if (collision.CompareTag("Enemy"))
        {
            EnemyGetHit enemyHit = collision.gameObject.GetComponent<EnemyGetHit>();

            if (stats["stun"]) { enemyHit.hitStun(meleeDamage); noEffects = false; }
            if (stats["burn"]) { enemyHit.hitBurn(meleeDamage); noEffects = false; }

            if (noEffects) { enemyHit.hit(meleeDamage); } //normal hit 
            if (!stats["arrow"]) { colided = true; } //if it's not an arrow destroy the projectile
        }
        else if (collision.CompareTag("Background")) { colided = true; }
    }
}
