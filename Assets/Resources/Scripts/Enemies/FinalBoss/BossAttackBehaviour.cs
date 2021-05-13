using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackBehaviour : MonoBehaviour
{
    public BossMeleeAttack meleeAttack;
    public BossShooting rangeAttack;
    public EnemyMoveScript movement;
    public Animator animator;
    private float rangeTimer = 0.0f, restTimer = 0.0f;
    private float rangeTime = 5.0f, restTime = 1.0f;
    private float randomFactor = 0.0f;
    private bool rest = false, charging = false;

    void Update() //if it's not shooting it's melee attacking
    {
        if (!charging)
        {
            rangeTimer += Time.deltaTime;
            if (rangeTimer >= (rangeTime + randomFactor))
            {
                ShootPrepare();
                rangeTimer = 0.0f;
                randomFactor = Random.Range(-1.5f, 1.5f);
            }
        }
        if (rest)
        {
            restTimer += Time.deltaTime;
            if (restTimer >= restTime) 
            {
                charging = false;
                rest = false;
                restTimer = 0.0f;
                animator.SetBool("walking", true);
                movement.Mobilize();
                meleeAttack.CanAttack();
            }
        }
    }

    private void ShootPrepare()
    {
        //Pre: ---
        //Post: starts the animation for shooting

        movement.Immobilize();
        animator.SetBool("walking", false);
        charging = true;
        meleeAttack.CannotAttack();
    }

    public void ShootAttack()
    {
        //Pre: ---
        //Post: shoots fireballs

        rangeAttack.Shoot();
        rest = true;
    }
}
