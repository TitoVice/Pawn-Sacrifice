using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    Dictionary<string, bool> stats;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //collision.gameObject.GetComponent<EnemyGetHit>().hit(effects);
            if (stats["arrow"] == false) { Destroy(gameObject); }
        }
        else if (collision.CompareTag("Background")) { Destroy(gameObject); }
    }

    public void getStats(Dictionary<string, bool> newStats)
    {
        //Pre: dictionary with stablished projectile characteristics
        //Post: dictionary copied

        stats = newStats;
    }
}
