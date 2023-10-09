using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnEnemyDeath : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public GameObject enemyTrigger;
    EnemyController enemyTriggerController;
    // Start is called before the first frame update
    void Start()
    {
        enemyToSpawn.SetActive(false);
        enemyTriggerController = enemyTrigger.GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyTriggerController.isDead){
            enemyToSpawn.SetActive(true);
        }
    }
}
