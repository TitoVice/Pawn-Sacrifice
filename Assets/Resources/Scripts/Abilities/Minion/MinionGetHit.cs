using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionGetHit : MonoBehaviour
{
    public int initialLife = 2;
    public int life = 2;
    private SpriteRenderer sprite;

    private bool damaged = false;
    private float damageTimer = 0.0f;
    private float damageCooldown = 0.6f;

    void Start()
    {
        sprite = transform.parent.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (damaged)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageCooldown) { damageTimer = 0.0f; damaged = false; }
        }
    }

    public void getHit()
    {
        //Pre: ---
        //Post: character receive damage, if life is 0 it¡s destroyed

        if (!damaged)
        {
            life -= 1;
            if (life <= 0) { transform.parent.GetComponent<Animator>().SetBool("dead", true); Destroy(gameObject); }
            else { StartCoroutine("Flash"); }
            
            damaged = true;
        }
    }

    IEnumerator Flash()
    {
        //coroutine to flash the minion as it's hit
        for (int i = 0; i < 3; i++){
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);  
        }
    }
}
