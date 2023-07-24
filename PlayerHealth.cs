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
    public static bool dead;
    
    // Start is called before the first frame update
    void Start()
    {
        dead = false;
        health = maxHealth;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int damage){
        health -= damage;
        if(health > 0){
            healthBar.fillAmount = health / 100;
        }
        else {
            healthBar.fillAmount = 0;
            StartCoroutine(Die());
        }
    }

    IEnumerator Die(){
        anim.SetTrigger("dieAnim");
        Instantiate(deathEffect, transform.position, transform.rotation);
        yield return new WaitForSeconds(1f);
        dead = true;
        //disable PlayerMovement2d
        GetComponent<PlayerMovement2D>().enabled =false;
    }
}
