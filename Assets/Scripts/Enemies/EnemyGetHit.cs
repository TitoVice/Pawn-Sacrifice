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
