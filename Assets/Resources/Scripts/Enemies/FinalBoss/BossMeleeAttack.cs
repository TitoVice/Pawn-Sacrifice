using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeAttack : MonoBehaviour
{
    private bool canAttack = true;
    private bool going = true;
    private float cooldownTimer = 0.0f, cooldownTime = 1.5f;
    public BoxCollider2D meleeCollider;
    public Transform initialPos, finalPos;

    void Update()
    {
        if (canAttack)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownTime) //time between attacks
            {
                meleeCollider.enabled = true;
                if (Vector3.Distance(transform.position, finalPos.position) <= 0.02f || !going) //melee returning
                {
                    transform.position = Vector3.MoveTowards(transform.position, initialPos.position, Time.deltaTime*7);
                    going = false;
                    if (Vector3.Distance(transform.position, initialPos.position) <= 0.02f) //arrived to initial pos
                        { 
                            going = true; 
                            transform.position = initialPos.position; 
                            meleeCollider.enabled = false;
                            cooldownTimer = 0.0f;
                        } 
                }
                else { transform.position = Vector3.MoveTowards(transform.position, finalPos.position, Time.deltaTime*7); } //melee going to the end
            }
        }
    }

    public void CanAttack()
    {
        //Pre: ---
        //Post: let's the boss attack

        canAttack = true;
    }
    public void CannotAttack()
    {
        //Pre: ---
        //Post: doesn't let the boss attack

        canAttack = false;
    }

    public void returnMeleeWeapon()
    {
        //Pre: ---
        //Post: make the weapon to return with the boss

        going = false;
    }
}
