using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeHit : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("HitDetector"))
        {
            collider.GetComponent<CharacterGetHit>().getHit();
        }
    }
}