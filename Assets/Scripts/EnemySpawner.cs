using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
	public static int enemyCount = 0;
	public GameObject enemyToSpawn;
	public float minSpawnTimer = 3f;
	public float maxSpawnTimer = 12f;
	public float minMoveSpeed = 5f;
	public float maxMoveSpeed = 15f;
	public int spawnWithinRadius= 20;

	void Start () {
		StartCoroutine(Spawn(Random.Range(minSpawnTimer, maxSpawnTimer)));	//calls the timed enemy spawner function
	}
	
	IEnumerator Spawn(float timeDelay) {

		yield return new WaitForSeconds(timeDelay);		//delay between spawning a new enemy
		if(enemyCount < GameSettings.maxEnemies){		//limits enemies on screen
			enemyCount++;
			Instantiate(enemyToSpawn, transform.position + Random.insideUnitSphere*spawnWithinRadius, Random.rotation);		//spawns an enemy
		}
		StartCoroutine(Spawn(Random.Range(minSpawnTimer, maxSpawnTimer)));		//starts the cycle over
	}
}
