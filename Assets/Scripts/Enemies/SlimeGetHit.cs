using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeGetHit : EnemyGetHit
{
    public GameObject slimePrefab;
    private bool splited = false;
    public bool canSplit = true;
    private Vector3 endPosition;
    private float timer = 0.0f;
    private float travelTime = 0.5f;

    public override void Death()
    {
        //Pre: ---
        //Post: splits the slime in 3 mini slimes

        GetComponent<BoxCollider2D>().enabled = false;
        animator.SetBool("dying", true); 

        if (GetComponent<EnemyMoveScript>().enabled) { GetComponent<EnemyMoveScript>().Immobilize(); }
        else if (GetComponent<SlimeMovementScript>().enabled) { GetComponent<SlimeMovementScript>().Immobilize(); } 
    }

    public override void extraAction(float deltaTime)
    {
        if (splited)
        {
            timer += deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPosition, deltaTime*7);
            if (timer >= travelTime) 
            { 
                splited = false; 
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
    
    public void getEndPosition(Vector3 end)
    {
        endPosition = end;
    }

    private void Split()
    {
        if (canSplit)
        {
            Vector3 target = GetComponent<EnemyMoveScript>().target.position;
            Vector3 direction = (transform.position - target).normalized;
            int rotation = -90;
            GameObject firstRegenerator = null;

            for (int i = 0; i < 3; i++)
            {
                GameObject miniSlime = Instantiate(slimePrefab, transform.position, transform.rotation);
                SlimeGetHit miniHit = miniSlime.GetComponent<SlimeGetHit>();

                miniSlime.GetComponent<BoxCollider2D>().enabled = false;

                miniSlime.transform.localScale = new Vector3(miniSlime.transform.localScale.x/2, miniSlime.transform.localScale.y/2, 1.0f);
                direction = Quaternion.Euler(0, 0, rotation) * direction;
                endPosition = new Vector3(direction.x*15, direction.y*15, direction.z);
                miniHit.getEndPosition(endPosition);

                miniHit.splited = true;
                miniHit.canSplit = false;
                miniHit.life /= 2; 
    
                rotation += 90;

                if (i != 0)
                {
                    if (firstRegenerator != null) 
                    { 
                        miniHit.Regenerator(firstRegenerator.transform); 
                        firstRegenerator.GetComponent<SlimeGetHit>().Regenerator(miniSlime.transform); 
                    }
                    else { firstRegenerator = miniSlime; } //gets the first slime who wants wants to generate a bigger slime
                }
            }
        }
        Destroy(gameObject);
    }

    public void Regenerator(Transform other)
    {
        //Pre: ---
        //Post: tries to make a bigger slime 

        gameObject.GetComponent<EnemyMoveScript>().enabled = false;
        SlimeMovementScript SMscript = gameObject.GetComponent<SlimeMovementScript>();
        SMscript.enabled = true;
        SMscript.getTarget(other);
    }
}
