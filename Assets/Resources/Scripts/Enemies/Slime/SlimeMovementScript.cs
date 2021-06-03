using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovementScript : EnemyMoveScript
{
    private float fusionTimer = 0.0f;
    private float launchTime = 1.0f;
    public float distance = 0.7f;
    private bool animating = false;
    public bool fusioner = false;
    public bool launching = true;
    private bool charAttack = false;

    public override void movement(float time)
    {
        if (charAttack) { base.movement(time); }
        else if (!charAttack && target != null) { agent.SetDestination(target.position); }

        lookDirection(target.position);
        if (target != null && !animating)
        {
            fusionTimer += time;
            if (Vector3.Distance(transform.position, target.position) < distance && fusionTimer >= launchTime)
            {
                animator.SetBool("fusioning", true);
                animating = true;
                if (!target.GetComponent<SlimeMovementScript>().fusioner) { fusioner = true; }
            }
        }
        else if (target == null && !launching) //in case the other slime is destroyed, it tries to attack the characters
        {
            animator.SetBool("fusioning", false);
            animating = false;
            charAttack = true;
            gameObject.GetComponent<EnemyMoveScript>().enabled = true;
            gameObject.GetComponent<SlimeMovementScript>().enabled = false;
        }
    }

    /*public override void getTarget(Transform objective)
    {
        print("ho intenta, "+objective.name);
        target = objective;
    }*/

    public void fusion()
    {
        //Pre: ---
        //Post: fusions two slimes
        
        //RoomBehavior roomBehavior = transform.parent.parent.GetComponent<RoomBehavior>();
        if (fusioner)
        {
            Destroy(target.gameObject);
            GetComponent<BoxCollider2D>().enabled = false;
            target.GetComponent<BoxCollider2D>().enabled = false;
            GameObject slime = Resources.Load<GameObject>("Prefabs/Enemies/Slime");
            GameObject bigSlime = Instantiate(slime, transform.position, transform.rotation, transform.parent);
            bigSlime.GetComponent<EnemyMoveScript>().characters = characters;

            SlimeGetHit getBigHit = bigSlime.GetComponent<SlimeGetHit>();
            SlimeGetHit getHit = GetComponent<SlimeGetHit>();

            getBigHit.life -= 2*(getHit.familyTree-1);
            getBigHit.familyTree = getHit.familyTree;
            //roomBehavior.EnemyListChange();
            Destroy(gameObject);
        }
    }
}
