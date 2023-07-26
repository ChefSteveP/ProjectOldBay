using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public GameObject impactEffect;
    public LineRenderer lineRenderer;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI storageText;
    public int damage = 20;
    public int maxAmmo = 10;
    private int currentAmmo;
    public int storageAmmo = 50;
    public float reloadTime = 2f;
    private bool isReloading = false;
    private Animator anim;
    private PlayerMovement2D playerMovement;

    private void Awake() {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement2D>();
    }

    private void Start() {
        currentAmmo = maxAmmo;
    }
    // Update is called once per frame
    void Update()
    {
        if(isReloading){
            ammoText.text = "Reloading...";
        } else {
            ammoText.text = "Bullets in clip: "+currentAmmo + "/" + maxAmmo;
            if(storageAmmo > 0){   
                storageText.text = "Bullets in Storage: "+ storageAmmo;
            } else {
                storageText.text = "Bullets in Storage: Out";
            }
        }
        
        
        if(!Pause.isGamePaused && !PlayerHealth.dead && !isReloading)
        {
            if(Input.GetButtonDown("Fire1") && currentAmmo > 0)
            {
                StartCoroutine(Shoot());
                currentAmmo--;
            }
            else if(Input.GetButtonDown("Fire1") && currentAmmo <= 0 && storageAmmo > 0)
            {
                StartCoroutine(Reload());
            }
        }
    }

    IEnumerator Shoot(){

       RaycastHit2D hitInfo =  Physics2D.Raycast(firePoint.position, firePoint.right);
       
       if(hitInfo)
       {
            EnemyController enemy = hitInfo.transform.GetComponent<EnemyController>();
            if(enemy)
            {
                enemy.TakeDamage(damage);
            }
            GameObject cloneImpact = Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
            
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
            Destroy(cloneImpact, 1f);
       }
       else 
       {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100);
       }
       lineRenderer.enabled = true;
       yield return new WaitForSeconds(0.02f);
       lineRenderer.enabled = false;
    }

    IEnumerator Reload(){
        isReloading = true;
        anim.SetBool("isReloading", true);
        playerMovement.speed = playerMovement.speed / 2;
        yield return new WaitForSeconds(reloadTime);
        anim.SetBool("isReloading", false);
        

        if(storageAmmo >= maxAmmo){
            storageAmmo -= maxAmmo;
            currentAmmo = maxAmmo;
        } else {
            currentAmmo = storageAmmo;
            storageAmmo = 0;
        }

        isReloading = false;  
        playerMovement.speed = playerMovement.speed * 2;
    }
}
