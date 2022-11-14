using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // change variable directly in Unity
    [SerializeField] private float speed;
    private Rigidbody2D body;

    // executed after script(in our case PlayerMovement) has been loaded
    // executed once
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // run every frame of the game
    private void Update()
    {
        // Vector2 (left/right, up/down) or just (x, y)
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);
        // if Vector3 (left/right, up/down, backwards/forwards) or just (x, y, z)

        // Jump while pressing "Space" or "ArrowUp" button
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
        {
            body.velocity = new Vector2(body.velocity.x, speed);
        }
    }
}
