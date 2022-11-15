using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // change variable directly in Unity
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    // executed after script(in our case PlayerMovement) has been loaded
    // executed once
    private void Awake()
    {
        // Grab references for rigidbody and animator 
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // run every frame of the game
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Flip player when moving left/right
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector2.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        // Set animator parameters
        // run if changes in horizontal input
        anim.SetBool("run", horizontalInput != 0);
        // send animator variable
        anim.SetBool("grounded", isGrounded());

        // Wall jump logic
        if (wallJumpCooldown > 0.2f)
        {

            // Vector2 (left/right, up/down) or just (x, y)
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            // if Vector3 (left/right, up/down, backwards/forwards) or just (x, y, z)

            // manSpider(no license to original name -_-) power logic
            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0; // not fall down
                body.velocity = Vector2.zero; // stuck on the wall
            }
            else
            {
                body.gravityScale = 3;// can be change to another, just to not levitate
            }

            // Jump while pressing "Space" or "ArrowUp" button
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
            {
                Jump();
            }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            // setting trigger and let animator to change animation
            anim.SetTrigger("jump");
        } 
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                // ToDo:: Need to add ledder logic like this body velocity
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                // turn opposite direction
                transform.localScale = new Vector2(-Mathf.Sign(transform.localScale.x), transform.localScale.y);
            } 
            else 
            {
                // push away from the wall
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            }

            wallJumpCooldown = 0;
        }
    }

    // Widget collider touched another collider 2d (one object touchs another, in our case player to ground)
    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            Vector2.down, // down
            0.1f,
            groundLayer
            );
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            new Vector2(transform.localScale.x, 0), // left right
            0.1f,
            wallLayer
            );
        return raycastHit.collider != null;
    }
}
