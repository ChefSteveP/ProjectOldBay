using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public int health = 100;
    public GameObject deathEffect;
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    public float noticeDistance;
    public Transform player;
    private float distance;
    private Animator anim;
    private bool facingRight = true;
    private bool dead = false;
    public float timeBtwShots;
    public float startTimeBtwShots;
    public GameObject projectile;
    private bool noticed = false;



    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        timeBtwShots = startTimeBtwShots;
    }
    void Update()
    {
        if(!dead){
            groundMove();
        }

        if(noticed && timeBtwShots <=0)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
        
    }

    public void TakeDamage(int damage){
        health -= damage;
        if(health <= 0){
            StartCoroutine(Die());
        }
    }
    // Update is called once per frame
    IEnumerator Die(){
        dead = true;
        anim.SetTrigger("dieAnim");
        Instantiate(deathEffect, transform.position, transform.rotation);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void Flip()
    {
        //Flip along the Y axis, this also flips all children of the character. 
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y,transform.localScale.z);
        //change direction
        facingRight = !facingRight;
    }
    private void groundMove(){
        distance = Vector2.Distance(transform.position, player.position);
        if(distance > stoppingDistance && distance < noticeDistance)
        {
            noticed = true;
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            anim.SetBool("isRunning", true);

            //flipping logic
            if(transform.position.x > player.position.x && facingRight)
            {
                Flip();
            }
            else if(transform.position.x < player.position.x && !facingRight)
            {
                Flip();
            }
        }
        else if(distance < stoppingDistance && distance > retreatDistance)
        {
            transform.position = this.transform.position;
            anim.SetBool("isRunning", false);
        }
        else if(distance < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
            anim.SetBool("isRunning", true);

            if(transform.position.x > player.position.x && !facingRight)
            {
                Flip();
            }
            else if(transform.position.x < player.position.x && facingRight)
            {
                Flip();
            }
        }

    }
}
