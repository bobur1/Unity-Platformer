using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // change variable directly in Unity
    [SerializeField] private float speed;
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;

    // executed after script(in our case PlayerMovement) has been loaded
    // executed once
    private void Awake()
    {
        // Grab references for rigidbody and animator 
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // run every frame of the game
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        // Vector2 (left/right, up/down) or just (x, y)
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        // if Vector3 (left/right, up/down, backwards/forwards) or just (x, y, z)

        // Flip player when moving left/right
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector2.one;
        } else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        // Jump while pressing "Space" or "ArrowUp" button
        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) && grounded)
        {
            Jump();
        }

        // Set animator parametrs
        anim.SetBool("run", horizontalInput != 0); // run if changes in horizontal input
        anim.SetBool("grounded", grounded); // send animator variable
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        // setting trigger and let animator to change animation
        anim.SetTrigger("jump");
        grounded = false;
    }

    // Widget collider touched another collider 2d (one object touchs another, in our case player to ground)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }
}
