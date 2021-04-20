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
        
        if (canSplit) { Split(); }
        Destroy(gameObject);
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

    /*public Rigidbody2D AddRigidBody()
    {
        //Pre: ---
        //Post: adds a rigidbody
        if (GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb2 = gameObject.AddComponent<Rigidbody2D>();

            rb2.bodyType.Equals("Dynamic");
            rb2.gravityScale = 0.0f;
            rb2.freezeRotation = true;
            
            return rb2;
        }   
        return null;
    }*/

    private void Split()
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
            //miniSlime.GetComponent<NavMeshAgent>().enabled = false;

            miniSlime.transform.localScale = new Vector3(0.3f, 0.3f, 1.0f);
            direction = Quaternion.Euler(0, 0, rotation) * direction;
            endPosition = direction*15;
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

    public void Regenerator(Transform other)
    {
        //Pre: ---
        //Post: tries to make a bigger slime 

        Destroy(GetComponent<EnemyMoveScript>());
        SlimeMovementScript SMscript = gameObject.AddComponent<SlimeMovementScript>();
        SMscript.getTarget(other);
    }
}
