using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public float speed = 10f;             //Floating point variable to store the player's movement speed.
    public float jumpForce = 6000f;

    private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.
    private Animator animator;
    public bool grounded = true;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        if (Input.GetButton("Jump") && grounded && rb2d.velocity.y <= 0)
        {
            rb2d.AddForce(new Vector2(0, jumpForce));
        }
        rb2d.velocity = new Vector2(moveHorizontal * speed, rb2d.velocity.y);
        if (rb2d.velocity.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            if (!animator.GetBool("Walking"))
            {
                animator.SetBool("Walking", true);
            }
        } else if (rb2d.velocity.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            if (!animator.GetBool("Walking"))
            {
                animator.SetBool("Walking", true);
            }
        } else
        {
            animator.SetBool("Walking", false);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        grounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        grounded = false;
    }
}
