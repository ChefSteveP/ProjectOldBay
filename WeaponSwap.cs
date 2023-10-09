using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    public static bool isHolstered = true;
    [SerializeField]
    public static bool haveGun = false;
    public GameObject weaponObject;


    // Update is called once per frame
    void Update()
    {
        if(!PlayerHealth.dead){
            if(Input.GetKeyDown(KeyCode.Q) && haveGun){
            isHolstered = !isHolstered;
            }
            if(isHolstered)
            {
                weaponObject.SetActive(false);
            } else {
                weaponObject.SetActive(true);
            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Gun")){
            haveGun = true;
            Destroy(other.gameObject);
        }
    }
}
