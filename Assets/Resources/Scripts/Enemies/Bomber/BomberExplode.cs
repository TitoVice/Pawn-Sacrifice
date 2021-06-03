using UnityEngine;

public class BomberExplode : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Animator animator;
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
        animator = transform.parent.GetComponent<Animator>();
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
    }

    private void StartExplosion(float deltaTime)
    {
        //Pre: ---
        //Post: starts the countdown for the explosion

        timer += deltaTime;
        if (timer >= explodeTime && !exploded)
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
        transform.parent.GetComponent<EnemyMoveScript>().Immobilize();
        transform.parent.GetComponent<Collider2D>().enabled = false;
        animator.SetBool("explode", true);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("HitDetector") || collider.CompareTag("Minion"))
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
            else if (collider.CompareTag("Minion"))
        {
            MinionGetHit minion = collider.gameObject.GetComponent<MinionGetHit>();

            minion.getHit();
        }
        }
    }
}
