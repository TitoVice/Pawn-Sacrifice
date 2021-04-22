using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGetHit : MonoBehaviour
{
    private SpriteRenderer sprite;
    public Animator animator;
    public float life = 5.0f;
    private float stunTime = 2.0f;

    private float cooldown = 7.0f;
    private float cooldownTimer = 0.0f;
    private bool startCooldown = false;
    private bool burned = false;
    private float burnTick = 2.0f;
    private float burnTimer = 0.0f;

    private float burnDamage = 1.0f;

    public Gradient gradient;
    public Vector3 hitPosition;

    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (startCooldown) 
        { 
            cooldownTimer += Time.deltaTime; 
            if (cooldownTimer >= cooldown) { startCooldown = false; cooldownTimer = 0.0f; } 
        }

        if (burned)
        {
            sprite.color = gradient.Evaluate((Mathf.Sin(Time.time)+1)/2); //blinks the color of the enemy
            burnTimer += Time.deltaTime; 
            if (burnTimer >= burnTick) 
            {  
                life -= burnDamage; 
                burnTimer = 0.0f; 
                if (life <= 0) { Death(); }
            } 
        }

        extraAction(Time.deltaTime);
    }

    public void colisionPosition(Vector3 pos)
    {
        //Pre: ---
        //Post: gets the point where they attacked

        hitPosition = pos;
        Vector3 target = GetComponent<EnemyMoveScript>().target.position;
        //Debug.DrawLine(transform.position, target, Color.red);
        Vector3 direction = new Vector3(transform.position.x - hitPosition.x, transform.position.y - hitPosition.y, transform.position.z).normalized;
        int rotation = 100;

        for (int i = 0; i < 3; i++)
        {
            Vector3 direction2 = new Vector3(Mathf.Cos(rotation)*direction.x - Mathf.Sin(rotation)*direction.y, Mathf.Sin(rotation)*direction.x + Mathf.Cos(rotation)*direction.y, direction.z);
            direction2 = new Vector3(direction2.x*5, direction2.y*5, direction.z);
            if (i == 0) {Debug.DrawRay(transform.position, direction2, Color.blue, 2f); rotation += 70;}
            if (i == 1) {Debug.DrawRay(transform.position, direction2, Color.yellow, 2f); rotation -= 140;}
            if (i == 2) {Debug.DrawRay(transform.position, direction2, Color.green, 2f);}

        }
    }

    public virtual void extraAction(float deltaTime)
    {
        //Pre:---
        //Post: function to do extra things
    }

    public void hit(float damage)
    {
        //Pre: ---
        //Post: gets a normal hit
        
        life -= damage;

        if (life <= 0) { Death(); }
        else
        {
            StartCoroutine("Flash");
        }
    }

    public void hitStun(float damage)
    {
        //Pre:---
        //Post: damage the enemy and stuns it

        life -= damage;
        if (life <= 0) { Death(); }
        else
        {
            if (!startCooldown)
            {
                //funcio per immobilitzar l'enemic
                StartCoroutine("Stuned");
                startCooldown = true;
            }
        }
    }

    public void hitBurn(float damage)
    {
        //Pre:---
        //Post: damage the enemy and burns it

        life -= damage;
        if (life <= 0) { Death(); }
        else
        {
            if (!burned)
            {
                burned = true;
            }
        }
    }

    public virtual void Death()
    {
        //Pre: ---
        //Post: destroys the gameObject

        Destroy(gameObject);
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

    IEnumerator Stuned()
    {
        //coroutine to mark the enemy as stuned
        sprite.color = Color.gray;
        yield return new WaitForSeconds(stunTime);
        sprite.color = Color.white;
        yield return new WaitForSeconds(0.0f);  
    }
}
