using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeGetHit : EnemyGetHit
{
    private GameObject slimePrefab;
    private bool splited = false;
    public bool canSplit = true;
    private Vector3 endPosition;
    private float timer = 0.0f;
    private float travelTime = 0.5f;
    public int familyTree = 1;

    public Transform toFollow;

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
        //pushes the slime when it's splitted
        if (splited)
        {
            timer += deltaTime;
            GetComponent<Rigidbody2D>().AddForce(endPosition);
            if (timer >= travelTime) 
            { 
                splited = false; 
                GetComponent<BoxCollider2D>().enabled = true;

                if (GetComponent<SlimeMovementScript>().enabled) { GetComponent<SlimeMovementScript>().getTarget(toFollow); GetComponent<SlimeMovementScript>().launching = false; }
                else if (GetComponent<EnemyMoveScript>().enabled) { GetComponent<EnemyMoveScript>().getTarget(null); }
                
                DestroyImmediate(GetComponent<Rigidbody2D>());
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
            GetComponent<BoxCollider2D>().enabled = false;
            slimePrefab = Resources.Load<GameObject>("Prefabs/Enemies/Slime");
            if (slimePrefab.GetComponent<SlimeGetHit>().life-2*familyTree > 0)//limit of times a slime can fuse
            {
                //Vector3 target = GetComponent<EnemyMoveScript>().target.position;
                Vector3 direction = new Vector3(transform.position.x - hitPosition.x, transform.position.y - hitPosition.y, transform.position.z).normalized;
                int rotation = 100;
                GameObject firstRegenerator = null;

                for (int i = 0; i < 3; i++)
                {
                    GameObject miniSlime = Instantiate(slimePrefab, transform.position, transform.rotation, transform.parent);
                    SlimeGetHit miniHit = miniSlime.GetComponent<SlimeGetHit>();

                    miniSlime.GetComponent<BoxCollider2D>().enabled = false;
                    miniHit.AddRigidBody();
                    miniSlime.GetComponent<EnemyMoveScript>().characters = GetComponent<EnemyMoveScript>().characters;

                    miniSlime.transform.localScale = new Vector3(miniSlime.transform.localScale.x/2, miniSlime.transform.localScale.y/2, 1.0f);

                    endPosition = new Vector3(Mathf.Cos(rotation)*direction.x - Mathf.Sin(rotation)*direction.y, Mathf.Sin(rotation)*direction.x + Mathf.Cos(rotation)*direction.y, direction.z);
                    endPosition = new Vector3(endPosition.x*5, endPosition.y*5, endPosition.z);
                    miniHit.getEndPosition(endPosition);

                    miniHit.splited = true;
                    miniHit.canSplit = false;
                    miniHit.life -= 2*familyTree; 
                    miniHit.familyTree = familyTree + 1;

                    if (i == 0) { rotation += 70; }
                    else if (i == 1) { rotation -= 140; }

                    if (i != 0)
                    {
                        if (firstRegenerator != null) 
                        { 
                            miniHit.Regenerator(firstRegenerator.transform); 
                            firstRegenerator.GetComponent<SlimeGetHit>().Regenerator(miniSlime.transform); 
                        }
                        else { firstRegenerator = miniSlime; } //gets the first slime who wants wants to generate a bigger slime
                    }
                    //else { miniSlime.GetComponent<EnemyMoveScript>().getTarget(null); }
                }
            }
        }
        //roomBehaviour.EnemyListChange();
        Destroy(gameObject);
    }

    public void AddRigidBody()
    {
        Rigidbody2D rb2 = gameObject.AddComponent<Rigidbody2D>();
        rb2.gravityScale = 0f;
        rb2.bodyType.Equals("Dynamic");
        rb2.freezeRotation = true;
    }

    public void Regenerator(Transform other)
    {
        //Pre: ---
        //Post: tries to make a bigger slime 
        
        SlimeMovementScript SMscript = GetComponent<SlimeMovementScript>();
        SMscript.enabled = true;
        SMscript.characters = GetComponent<EnemyMoveScript>().characters;
        //SMscript.getTarget(other);
        GetComponent<EnemyMoveScript>().enabled = false;
        toFollow = other;
    }
}
