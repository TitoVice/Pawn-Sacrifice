using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovementScript : EnemyMoveScript
{
    private float timer = 0.0f;
    private float launchTime = 1.0f;
    public float distance = 0.7f;
    public override void movement(float time)
    {
        if (target != null)
        {
            base.movement(time);

            timer += time;
            if (Vector3.Distance(transform.position, target.position) < distance && timer >= launchTime)
            {
                GetComponent<BoxCollider2D>().enabled = false;
                target.GetComponent<BoxCollider2D>().enabled = false;
                GameObject slime = (GameObject)Resources.Load("Prefabs/enemypocho", typeof(GameObject));
                Instantiate(slime, transform.position, transform.rotation);
                Destroy(gameObject);
                Destroy(target.gameObject);
            }
        }
        else //in case the other slime is destroyed, it tries to attack the characters
        {
            gameObject.AddComponent<EnemyMoveScript>();
            Destroy(GetComponent<SlimeMovementScript>());
        }
        
    }
}
