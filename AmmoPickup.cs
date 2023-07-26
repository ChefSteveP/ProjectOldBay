using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int bulletCount = 10;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            other.gameObject.GetComponent<Gun>().storageAmmo += bulletCount;
            Destroy(gameObject);
        }
    }
}
