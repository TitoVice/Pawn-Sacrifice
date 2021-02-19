using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D RB2;
    private Transform player;

    Vector2 movement;
    private void Start()
    {
        RB2 = GetComponent<Rigidbody2D>();
        player = GetComponent<Transform>();
    }

    void Update()
    {
        //WASD i fletxes
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        RB2.MovePosition(RB2.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void spawn(int[] spawn)
    {
        transform.position = new Vector3(spawn[0], spawn[1], 0);
    }
}
