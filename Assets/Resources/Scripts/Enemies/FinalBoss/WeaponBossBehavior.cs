using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBossBehavior : MonoBehaviour
{
    public BossMeleeAttack weapon;
    public BoxCollider2D colider;
    private float timer = 0.0f;
    private float coolDown = 1.0f;
    private float retard = 0.06f;
    private bool colided = false;

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

        void OnTriggerEnter2D(Collider2D collision)
    {
        //Pre:---
        //Post: hits the enemy with a melee attack
        
        if (collision.CompareTag("HitDetector"))
        {
            CharacterGetHit character = collision.gameObject.GetComponent<CharacterGetHit>();

            character.getHit();
            colided = true; 
            colider.enabled = false;
        }
        else if (collision.CompareTag("Background")) { colided = true; }
        else if (collision.CompareTag("Minion"))
        {
            MinionGetHit minion = collision.gameObject.GetComponent<MinionGetHit>();

            minion.getHit();
            colided = true; 
            colider.enabled = false;
        }
    }
}
