using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject fist;
    private Animator anim;

    ///I want to swap the melee attack out for the gun when i hit W

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Punch if the player presses the E key
            if(Input.GetKeyDown(KeyCode.E)){
                anim.SetTrigger("punch");
                StartCoroutine(fist.GetComponent<MeleeAttack>().Punch());
            }
    }
    
}
