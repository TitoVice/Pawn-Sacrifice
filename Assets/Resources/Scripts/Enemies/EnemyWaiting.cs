using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaiting : MonoBehaviour
{
    public EnemyMoveScript moveScript;
    public ChargerMovementScript chargerMovementScript;

    void Start()
    {
        if (moveScript != null) { moveScript.enabled = false; }
        else if (chargerMovementScript != null) { chargerMovementScript.enabled = false; }
    }

    public void stopWaiting()
    {
        if (moveScript != null) { moveScript.enabled = true; }
        else if (chargerMovementScript != null) { chargerMovementScript.enabled = true; }
    }
}
