using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 100;
    [HideInInspector] public bool isDead = false;
    public GameObject deathEffect;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     //Apply Damage to Enemy
    public void TakeDamage(int damage){
        health -= damage;
        if(health <= 0 && !isDead){
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
}
