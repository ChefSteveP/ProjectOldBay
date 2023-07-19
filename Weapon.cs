using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public int damage = 20;
    public GameObject impactEffect;

    public LineRenderer lineRenderer;

    private void Awake() {
       // PauseController =  GameObject.FindGameObjectWithTag("PauseBtn").GetComponent<Pause>();
    }
    // Update is called once per frame
    void Update()
    {
        if(!Pause.isGamePaused)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                StartCoroutine(Shoot());
            }  
        }
    }

    IEnumerator Shoot(){

       RaycastHit2D hitInfo =  Physics2D.Raycast(firePoint.position, firePoint.right);
       
       if(hitInfo)
       {
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
            
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
            
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
}
