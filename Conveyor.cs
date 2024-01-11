using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    // Rigidbody2D rb;
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private bool movingRight = true;

    private void OnCollisionStay2D(Collision2D other) {
        
        int direction = movingRight ? -1 : 1;
        if(other.gameObject.tag == "Player") {
            Vector2 velocityBoost = 10 * direction * transform.up * speed * Time.deltaTime;
            velocityBoost.y = 0;
        
            other.gameObject.GetComponent<PlayerMovement2D>().IncreaseSpeed(velocityBoost);
        }
        if(other.gameObject.tag != "Player"){
            Vector2 newPositionVector = (transform.position + direction * transform.up * (GetComponent<SpriteRenderer>().bounds.size.x/2 + 5) - other.transform.position) * speed * Time.deltaTime;
        
            other.transform.GetComponent<Rigidbody2D>().velocity = newPositionVector;
        }
    }
}
