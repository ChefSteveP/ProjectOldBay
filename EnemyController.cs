using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 100;
    [HideInInspector] public bool isDead = false;
    public GameObject deathEffect;
    private Animator anim;
    public int animiationDelay = 1;

    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Animator>()){anim = GetComponent<Animator>();}
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
        if(anim != null){
            anim.SetTrigger("dieAnim");
        }
        
        GameObject cloneDeathEffect = Instantiate(deathEffect, transform.position, transform.rotation);
        yield return new WaitForSeconds(animiationDelay);
        Destroy(gameObject);
        Destroy(cloneDeathEffect, 1f);
    }
}
