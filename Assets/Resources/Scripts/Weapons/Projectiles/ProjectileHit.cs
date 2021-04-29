using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    Dictionary<string, bool> stats;
    private float projectileDamage;
    private bool noEffects = true;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyGetHit enemyHit = collision.gameObject.GetComponent<EnemyGetHit>();

            if (stats["stun"]) { enemyHit.hitStun(projectileDamage); noEffects = false; }
            if (stats["burn"]) { enemyHit.hitBurn(projectileDamage); noEffects = false; }

            if (noEffects) { enemyHit.hit(projectileDamage); } //normal hit 
            if (!stats["arrow"]) { Destroy(gameObject); } //if it's not an arrow destroy the projectile
        }
        else if (collision.CompareTag("Background")) { Destroy(gameObject); }
    }

    public void getStats(Dictionary<string, bool> newStats, float damage)
    {
        //Pre: dictionary with stablished projectile characteristics
        //Post: dictionary copied

        stats = newStats;
        projectileDamage = damage;
    }
}
