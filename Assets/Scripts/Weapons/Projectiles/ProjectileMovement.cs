using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    private Vector3 direction;
    private bool movable = false;
    public float speed = 10;

    void FixedUpdate()
    {
        if (movable) { transform.position += direction * speed * Time.fixedDeltaTime; }
    }

    public void getShotDirection(Vector3 direct)
    {
        direction = direct;
        movable = true;
    }
}
