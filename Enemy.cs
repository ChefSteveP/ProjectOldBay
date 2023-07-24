using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public enum AISTATE {PATROL=0, CHASE=1, ATTACK=2, RETREAT=3};
    public Transform player;
    public GameObject deathEffect;
    public GameObject projectile;
    public GameObject[] Waypoints = null;
    private Animator anim;
    public int enemyHealth = 100;
    public float runSpeed;
    public float patrolSpeed;
    public float attackDistance;
    public float retreatDistance;
    public float lostDistance;
    public float timeBtwShots;
    public float startTimeBtwShots;
    private bool facingRight = true;
    private bool isDead = false;
 

    public AISTATE CurrentState {
        get {
            return _CurrentState;
        }
        set {
            StopAllCoroutines();
            _CurrentState = value;

            switch(CurrentState){
            case AISTATE.PATROL:
                StartCoroutine(StatePatrol());
                break;
            case AISTATE.CHASE:
                StartCoroutine(StateChase());
                break;
            case AISTATE.ATTACK:
                StartCoroutine(StateAttack());
                break;
            case AISTATE.RETREAT:
                StartCoroutine(StateRetreat());
                break;
            }
        }
    }
    [SerializeField]
    private AISTATE _CurrentState = AISTATE.PATROL;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }
    void Start() {
        timeBtwShots = startTimeBtwShots;
        CurrentState = AISTATE.PATROL;
    }

    //Apply Damage to Enemy
    public void TakeDamage(int damage){
        enemyHealth -= damage;
        if(enemyHealth <= 0){
            StartCoroutine(Die());
        }
    }

    //On death, create death animation effects destroy object
    IEnumerator Die(){
        isDead = true;
        anim.SetTrigger("dieAnim");
        GameObject cloneDeathEffect = Instantiate(deathEffect, transform.position, transform.rotation);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        Destroy(cloneDeathEffect, 1f);
    }

    private void Flip(){

        //Flip along the Y axis, this also flips all children of the character. 
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y,transform.localScale.z);
        //change direction
        facingRight = !facingRight;
    }

    //Enemy will move around semi-randomly. Will do this until player enters their sight.
    public IEnumerator StatePatrol(){
        
        float TargetDistance = 2f;
        anim.SetBool("isRunning", true);

        //Choose random point to visit among list of patrol points
        Vector2 CurrentWaypoint = transform.position;
        if(Waypoints.Length > 0){
            CurrentWaypoint = Waypoints[Random.Range(0, Waypoints.Length)].transform.position;
        }

        while(CurrentState == AISTATE.PATROL){
            //Don't move if there is no waypoints;
            if(Waypoints.Length > 0){
                transform.position = Vector2.MoveTowards(transform.position, CurrentWaypoint, patrolSpeed * Time.deltaTime);
                if(transform.position.x > CurrentWaypoint.x && facingRight){
                    Flip();
                }
                else if(transform.position.x < CurrentWaypoint.x && !facingRight){
                    Flip();
                }
                //When in range of goal, pick a new point to move towards
                if(Vector3.Distance(transform.position, CurrentWaypoint) < TargetDistance){
                    CurrentWaypoint = Waypoints[Random.Range(0, Waypoints.Length)].transform.position;
                }
            }
            yield return null;
        }
    }

    //When Enemy sees player chase them to get in shooting range. 
    public IEnumerator StateChase(){

        while(CurrentState == AISTATE.CHASE){
            anim.SetBool("isRunning", true);

            //If Enemy gets close enough, stop and attack
            if(Vector3.Distance(transform.position, player.position) < attackDistance){
                CurrentState = AISTATE.ATTACK;
                yield break;
            }
            //If enemy gets too far away, stop chasing
            if(Vector3.Distance(transform.position, player.position) > lostDistance){
                CurrentState = AISTATE.PATROL;
                yield break;
            }
            transform.position = Vector2.MoveTowards(transform.position, player.position, runSpeed * Time.deltaTime);
            if(transform.position.x > player.position.x && facingRight){
                Flip();
            }
            else if(transform.position.x < player.position.x && !facingRight){
                Flip();
            }
            yield return null;
        }
    }

    //When in shooting range, start firing.
    public IEnumerator StateAttack(){

        while(CurrentState == AISTATE.ATTACK){
            //If player is getting away run after them.
            if(Vector3.Distance(transform.position, player.position) > attackDistance){
                CurrentState = AISTATE.CHASE;
                yield break;
            }
            //If player is too close back up
            // if(Vector3.Distance(transform.position, player.position) < retreatDistance){
            //     CurrentState = AISTATE.RETREAT;
            //     yield break;
            // }
            //Stop Running
            anim.SetBool("isRunning", false);
            //Shoot
            if(!isDead && !PlayerHealth.dead && timeBtwShots <=0){
                Instantiate(projectile, transform.position, Quaternion.identity);
                timeBtwShots = startTimeBtwShots;
            }
            else {
                timeBtwShots -= Time.deltaTime;
            }
            yield return null;
        }
    }

    public IEnumerator StateRetreat(){

        while(CurrentState == AISTATE.RETREAT){
            anim.SetBool("isRunning", true);

            //If proper retreat distance has been reached start shooting
            if(Vector3.Distance(transform.position, player.position) > retreatDistance){
                CurrentState = AISTATE.ATTACK;
                yield break;
            }
            transform.position = Vector2.MoveTowards(transform.position, player.position, -runSpeed * Time.deltaTime);

            //Flip when appropriate
            if(transform.position.x > player.position.x && facingRight){
                Flip();
            }
            else if(transform.position.x < player.position.x && !facingRight){
                Flip();
            }
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            CurrentState = AISTATE.CHASE;
        }
    }
}
