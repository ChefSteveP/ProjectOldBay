using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement2D : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public float crouchDampening = 1f;
    private Animator anim;
    Rigidbody2D rb;
    ReloadScene reloadScene;
    public GameObject critZone;
    bool isGrounded = true;
    bool shouldJump = false;
    bool facingRight = true;
    public GameObject gun;
    public GameObject keyUI;
    public float horizontalInput = 0f;
    public Vector2 externalSpeed;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        reloadScene = GetComponent<ReloadScene>();
        externalSpeed = new Vector2(0f,0f);
    }

    void Update()
    {
        if(!Pause.isGamePaused && !PlayerHealth.dead)
        {
            // Move the player left and right using the A and D keys, flipping the character along the Y axis when turning.
            horizontalInput = Input.GetAxisRaw("Horizontal");

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
                shouldJump = true;
                isGrounded = false;
                anim.SetBool("landed", false);
                anim.SetTrigger("jump");
            }
            if((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && isGrounded){
                if(!anim.GetBool("isCrouching")){
                    anim.SetBool("isCrouching", true);
                    critZone.transform.position += new Vector3(0f,-0.5f,0f);
                    speed = crouchDampening * speed;
                }
            }
            else{
                bool ObjectAbove = Physics2D.Raycast(transform.position + new Vector3(0.5f,0f,0f), Vector2.up, 2f).collider != null || Physics2D.Raycast(transform.position + new Vector3(-0.5f,0f,0f), Vector2.up, 2f).collider != null;
                if(anim.GetBool("isCrouching") && !ObjectAbove){
                    anim.SetBool("isCrouching", false);
                    speed = speed / crouchDampening;
                    critZone.transform.position += new Vector3(0f,0.5f,0f);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Stop conveyr momentum when landing on other surfaces
        // if(collision.gameObject.tag != "Conveyor"){
        //         IncreaseSpeed(new Vector2(0f,0f));
        //     }
        // Check if the player has landed on the ground
        if (!PlayerHealth.dead && (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Conveyor"))
        {
            isGrounded = true;
            anim.SetBool("landed", true);
        }
        if (!PlayerHealth.dead && collision.gameObject.name == "Key")
        {
            keyUI.SetActive(true);
            Destroy(collision.gameObject);
        }
        if (!PlayerHealth.dead && collision.gameObject.name == "Door" && keyUI.activeSelf)
        {
            //fade to black
            //Load DockWarkehouse
            reloadScene.FadeToLevel(2);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        IncreaseSpeed(new Vector2(0f,0f));
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(horizontalInput * speed * Time.deltaTime, rb.velocity.y) + externalSpeed;

        if(shouldJump) {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            shouldJump = false;
        }
    }

    public void IncreaseSpeed(Vector2 speed) {
        // if(externalSpeed == new Vector2(0f,0f)){
        //     externalSpeed = speed;
        // }
        externalSpeed = speed;
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
