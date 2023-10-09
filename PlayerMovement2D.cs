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
    public GameObject critZone;
    bool isGrounded = true;
    bool facingRight = true;
    public GameObject gun;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(!Pause.isGamePaused && !PlayerHealth.dead)
        {
            // Move the player left and right using the A and D keys, flipping the character along the Y axis when turning.
            float horizontalInput = Input.GetAxis("Horizontal");

            //holstered movement faces the movment direction
            if(WeaponSwap.isHolstered){
                if(!facingRight && horizontalInput > 0)
                {
                    Flip();
                }
                if(facingRight && horizontalInput < 0)
                {
                    Flip();
                }
            } 
            //aiming movement faces the gun direction
            else {
                if(Aiming.mousePos.x < transform.position.x)
                {
                    if(facingRight){
                        Flip();
                        FlipGun();
                    }
                    if(horizontalInput > 0){
                        anim.SetBool("isBackward", true);
                    }
                    else{
                        anim.SetBool("isBackward", false);
                    }
                }
                if(Aiming.mousePos.x > transform.position.x)
                {
                    if(!facingRight){
                        Flip();
                        FlipGun();
                    }
                    if(horizontalInput < 0){
                        anim.SetBool("isBackward", true);
                    }
                    else{
                        anim.SetBool("isBackward", false);
                    }
                }
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
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded )
            {
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                isGrounded = false;
                anim.SetBool("landed", false);
                anim.SetTrigger("jump");
            }
            if((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && isGrounded){
                if(!anim.GetBool("isCrouching")){
                    anim.SetBool("isCrouching", true);
                    critZone.transform.position += new Vector3(0f,-0.5f,0f);
                }
            }
            else{
                if(anim.GetBool("isCrouching")){
                    anim.SetBool("isCrouching", false);
                    critZone.transform.position += new Vector3(0f,0.5f,0f);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player has landed on the ground
        if (!PlayerHealth.dead && collision.gameObject.tag == "Ground")
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

    //flips the gun along the X axis to correct for rotating above and below the player.
    private void FlipGun()
    {
        //Flip along the Y axis, this also flips all children of the character. Like our firepoint.
        gun.transform.Rotate(180f,0f,0f);
    }
}
