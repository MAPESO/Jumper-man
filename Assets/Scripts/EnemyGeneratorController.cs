using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneratorController : MonoBehaviour {

	/* 
	   Mechanics of the Enemy Generator
	   
       Este script genera enemigos de manera secuencial, usadno el prefab "Enemy".
     */


	// GameObject

	public GameObject enemyPrefab;

	// Normal data type

	public float generatorTimer = 1.74f;

    public void CreateEnemy() {
        Instantiate(enemyPrefab, /* posicion actual del GameObject(tiene enlazado el script) */ transform.position, /* se pasa obligatoriamente */ Quaternion.identity);
	}
    	

    public void StartGenerator() {
        InvokeRepeating( /* Metodo */ "CreateEnemy", /* Tiempo de retardo, cuando se ejecuta por primera vez*/0f,  /* Tiempo en generarse */generatorTimer);
    }

    public void StopGenerator(bool clean = false) {
        CancelInvoke("CreateEnemy");
        if (clean) {
            Object[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject enemy in allEnemies) {
                Destroy(enemy);
            }
        }
    }

}
