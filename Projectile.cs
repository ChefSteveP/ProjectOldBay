using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform player;
    private Vector2 target;
    public GameObject impactEffect;
    public Rigidbody2D rb;
    public float force;
    public float heightOffset;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        target = new Vector2(player.position.x, player.position.y+ heightOffset);

        rb = GetComponent<Rigidbody2D>();
        Vector2 dist = new Vector2(target.x - transform.position.x, target.y - transform.position.y);
        rb.AddForce((dist/dist.magnitude)* force, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().takeDamage(10);
            DestroyProjectile();       
        }
        else if(other.CompareTag("PlayerCrit"))
        {
            other.GetComponentInParent<PlayerHealth>().takeDamage(50);
            DestroyProjectile();       
        }
        else if(!other.CompareTag("Enemy") && !other.CompareTag("Projectile") && !other.CompareTag("EnemyCrit") && !other.CompareTag("ProjectileIgnore"))
        {
            DestroyProjectile();
        }
    }
    void DestroyProjectile()
    {
        GameObject impactEffectClone = Instantiate(impactEffect,transform.position, Quaternion.identity);
        Destroy(impactEffectClone, 1f);
        Destroy(gameObject);
    }
}
