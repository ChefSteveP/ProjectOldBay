using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundPatrol : MonoBehaviour
{
    // Start is called before the first frame update
    public enum AISTATE {PATROL=0, CHASE=1, ATTACK=2};
    public Transform player;
    public Transform enemyPrefab;
    public GameObject projectile;
    public GameObject[] Waypoints = null;
    private Animator anim;
    private EnemyController enemyController;
    public GameObject fist;
    public float runSpeed;
    public float patrolSpeed;
    public float attackDistance;
    public float lostDistance;
    public float timeBtwShots;
    public float startTimeBtwShots;
    [SerializeField]
    private bool facingRight = true;
    public bool stationary = false;
 
    public enum ATTACKTYPE {MELEE=0, RANGED=1};

    public ATTACKTYPE AttackType {
        get {
            return _AttackType;
        }
        set {
            _AttackType = value;
        }
    }
    [SerializeField] private ATTACKTYPE _AttackType = ATTACKTYPE.MELEE;
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
            }
        }
    }
    [SerializeField] private AISTATE _CurrentState = AISTATE.PATROL;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
    }
    void Start() {
        timeBtwShots = startTimeBtwShots;
        CurrentState = AISTATE.PATROL;
        if(AttackType == ATTACKTYPE.MELEE){
            attackDistance = 1.5f;
            
        }
        else if (AttackType == ATTACKTYPE.RANGED){
            attackDistance = 10f;
        }
        Physics2D.IgnoreLayerCollision(3, 3);
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

        while(CurrentState == AISTATE.PATROL && !enemyController.isDead){
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

        while(CurrentState == AISTATE.CHASE && !enemyController.isDead){
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

        while(CurrentState == AISTATE.ATTACK && !enemyController.isDead){
            //If player is getting away run after them.
            if(Vector3.Distance(transform.position, player.position) > attackDistance){
                CurrentState = AISTATE.CHASE;
                yield break;
            }

            //Stop Running
            anim.SetBool("isRunning", false);

            //face player
            if(transform.position.x > player.position.x && facingRight){
                Flip();
            }
            else if(transform.position.x < player.position.x && !facingRight){
                Flip();
            }
            
            //Ranged Attacl
            if(AttackType == ATTACKTYPE.RANGED){
                //Shoot
                if(!enemyController.isDead && !PlayerHealth.dead && timeBtwShots <=0){
                    Instantiate(projectile, transform.position, Quaternion.identity);
                    timeBtwShots = startTimeBtwShots;
                }
                else if(PlayerHealth.dead) {
                    CurrentState = AISTATE.PATROL;
                    yield break;
                }
                else {
                    timeBtwShots -= Time.deltaTime;
                }
            }
            //Melee Attack
            else if (AttackType == ATTACKTYPE.MELEE){
                //Melee
                if(!enemyController.isDead && !PlayerHealth.dead){
                    //anim
                    //wait 0.5 seconds before attacking
                    yield return new WaitForSeconds(0.5f);
                    if(!enemyController.isDead && !PlayerHealth.dead){
                        anim.SetTrigger("punch");
                        StartCoroutine(fist.GetComponent<MeleeAttack>().Punch());
                    }
                }
                else if(PlayerHealth.dead) {
                    CurrentState = AISTATE.PATROL;
                    yield break;
                }
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
