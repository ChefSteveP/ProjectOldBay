using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement2D : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    private Animator anim;

    Rigidbody2D rb;
    bool isGrounded = true;
    bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Move the player left and right using the A and D keys, flipping the character along the Y axis when turning.
        float horizontalInput = Input.GetAxis("Horizontal");
        if(!facingRight && horizontalInput > 0)
        {
            Flip();
        }
        if(facingRight && horizontalInput < 0)
        {
            Flip();
        }
        transform.position += new Vector3(horizontalInput * speed * Time.deltaTime, 0, 0);

        //Change the Animation boolean
        if(horizontalInput == 0)
        {
            anim.SetBool("isRunning", false);
        }
        else 
        {
             anim.SetBool("isRunning", true);
        }
        // Jump if the player is on the ground and presses the space bar
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
            anim.SetBool("landed", false);
            anim.SetTrigger("jump");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player has landed on the ground
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            anim.SetBool("landed", true);
        }
    }

    private void Flip()
    {
        //Flip along the Y axis, this also flips all children of the character. Like our firepoint.
        transform.Rotate(0f,180f,0f);
        //change direction
        facingRight = !facingRight;
    }
}
