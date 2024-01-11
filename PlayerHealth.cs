using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    private float health;
    public Image healthBar;
    public GameObject player;
    private Animator anim;
    public GameObject deathEffect;
    Rigidbody2D rb;
    public GameObject RetryMenu;
    public static bool dead;
    
    // Start is called before the first frame update
    void Start()
    {
        dead = false;
        health = maxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    public void takeDamage(int damage){
        health -= damage;
        if(health > 0){
            healthBar.fillAmount = health / maxHealth;
        }
        else {
            healthBar.fillAmount = 0;
            if(!dead){
                StartCoroutine(Die());
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        GameObject otherObj = other.gameObject;
        //if the player collides with out of bounds, kill them
        if(otherObj.CompareTag("OutOfBounds")){
            if(!dead){
                Debug.Log("Player out of bounds");
                takeDamage(100);
            }
        }
    }

    IEnumerator Die(){
        anim.SetTrigger("dieAnim");
        GameObject cloneDeathEffect = Instantiate(deathEffect, transform.position, transform.rotation);
        dead = true;
        //disable PlayerMovement2d
        GetComponent<PlayerMovement2D>().enabled = false;        
        Destroy(cloneDeathEffect, 1f);

        //prompt retry menu
        RetryMenu.SetActive(true);
        yield return new WaitForSeconds(1f);
        WeaponSwap.haveGun = false;

        
        
        //ReloadScene.ReloadLevel();
    }
}
