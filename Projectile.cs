using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
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
            PlayerHealth playerH = other.gameObject.GetComponent<PlayerHealth>();
            playerH.takeDamage(10);            //Take health DestroyProjectile();
        }
        if(!other.CompareTag("Enemy") && !other.CompareTag("Projectile"))
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
