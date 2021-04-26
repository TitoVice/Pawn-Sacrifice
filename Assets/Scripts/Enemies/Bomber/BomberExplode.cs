using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberExplode : MonoBehaviour
{
    private SpriteRenderer sprite;
    private CircleCollider2D radius;
    private float intialAlpha, finalAlpha;
    private bool inRadius = false;
    private bool exploded = false;
    private float timer = 0.0f;
    private float explodeTime = 3.5f;


    private float ti = 0f;
    private float xd = 0.2f;
    
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        radius = GetComponent<CircleCollider2D>();
        intialAlpha = 0.3f;
        finalAlpha = 1f;
    }

    void Update()
    {
        if (inRadius)
        {
            StartExplosion(Time.deltaTime);
        }
        if (exploded) {                             //canviar aixo per fer la funcio despres de l'animacio
            ti +=Time.deltaTime;
            if (ti >= xd) { AutoDestroy();}
        }
    }

    private void StartExplosion(float deltaTime)
    {
        //Pre: ---
        //Post: starts the countdown for the explosion

        timer += deltaTime;
        if (timer >= explodeTime)
        {
            Explode();
        }
        else
        {
            Color tmp = sprite.color;
            tmp.a = (Mathf.Sin(Time.time)+1.5f)/2;
            sprite.color = tmp;
        }
    }

    private void Explode()
    {
        //Pre: ---
        //Post: does damage to whom is in the radius and kills himself

        exploded = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("HitDetector"))
        {
            inRadius = true;
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (exploded)
        {
            if (collider.CompareTag("HitDetector"))
            {
                collider.GetComponent<CharacterGetHit>().getHit();
            }
        }
    }

    public void AutoDestroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
