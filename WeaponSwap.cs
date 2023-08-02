using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    public static bool isHolstered = true;
    public GameObject weaponObject;


    // Update is called once per frame
    void Update()
    {
        if(!PlayerHealth.dead){
            if(Input.GetKeyDown(KeyCode.Q)){
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
}
