using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public int damage;
    private BoxCollider2D hitbox;
    private void Awake() {
        hitbox = gameObject.GetComponent<BoxCollider2D>();
        hitbox.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        GameObject opponent = other.gameObject;
        //If the opponent is an enemy, apply damage
        if(opponent.GetComponent<EnemyController>() && !gameObject.GetComponent<EnemyController>()){
            opponent.GetComponent<EnemyController>().TakeDamage(damage);
        }
        //If the opponent is the player, apply damage
        else if(opponent.GetComponent<PlayerHealth>() && !gameObject.GetComponent<PlayerHealth>()){
            opponent.GetComponent<PlayerHealth>().takeDamage(damage);
        }
    }

    public IEnumerator Punch(){
        hitbox.enabled= true;
        yield return new WaitForSeconds(0.25f);
        hitbox.enabled = false;
    }
}
