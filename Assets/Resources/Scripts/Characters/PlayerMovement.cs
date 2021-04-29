using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Animator animator;
    private SpriteRenderer sprite;
    private Rigidbody2D RB2;
    private Transform player;
    private bool stopped = false;

    Vector2 movement;
    private void Start()
    {
        RB2 = GetComponent<Rigidbody2D>();
        player = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!stopped)
        {
            //WASD i fletxes
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (movement.x != 0 || movement.y != 0) 
            { 
                animator.SetBool("moving", true); 
                if (movement.x == -1 && !sprite.flipX) { sprite.flipX = true; } //flip the sprite to the left
                else if (movement.x == 1 && sprite.flipX) { sprite.flipX = false; } //flip the sprite to the right
            }
            else { animator.SetBool("moving", false); }
        }
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    void FixedUpdate()
    {
        if (!stopped)
        {
            RB2.MovePosition(RB2.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void Immobilize()
    {
        stopped = true;
    }
    public void Mobilize()
    {
        stopped = false;
    }
}
