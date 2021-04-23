using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovementScript : EnemyMoveScript
{
    private float timer = 0.0f;
    private float launchTime = 1.0f;
    public float distance = 0.7f;
    private bool animating = false;
    public bool fusioner = false;

    public override void movement(float time)
    {
        if (target != null && !animating)
        {
            base.movement(time);

            timer += time;
            if (Vector3.Distance(transform.position, target.position) < distance && timer >= launchTime)
            {
                animator.SetBool("fusioning", true);
                animating = true;
                if (!target.GetComponent<SlimeMovementScript>().fusioner) { fusioner = true; }
            }
        }
        else if (target == null) //in case the other slime is destroyed, it tries to attack the characters
        {
            animator.SetBool("fusioning", false);
            animating = false;
            gameObject.GetComponent<EnemyMoveScript>().enabled = true;
            gameObject.GetComponent<SlimeMovementScript>().enabled = false;
        }
    }

    public void fusion()
    {
        //Pre: ---
        //Post: fusions two slimes
        
        if (fusioner)
        {
            Destroy(target.gameObject);
            GetComponent<BoxCollider2D>().enabled = false;
            target.GetComponent<BoxCollider2D>().enabled = false;
            GameObject slime = Resources.Load<GameObject>("Prefabs/Enemies/Slime");
            GameObject bigSlime = Instantiate(slime, transform.position, transform.rotation);

            SlimeGetHit getBigHit = bigSlime.GetComponent<SlimeGetHit>();
            SlimeGetHit getHit = GetComponent<SlimeGetHit>();

            getBigHit.life -= 2*(getHit.familyTree-1);
            getBigHit.familyTree = getHit.familyTree;

            Destroy(gameObject);
        }
    }
}
