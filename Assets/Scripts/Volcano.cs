using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Volcano : MonoBehaviour {
	
	//variables
	public GameObject eruptionRock;
	public float minSpawnTimer = .1f;
	public float maxSpawnTimer = .5f;
	public int spawnWithinRadius = 10;
	public float thrust = 100f;
	public float thrustDuration = 0.1f;
	
	private float thrustEnd;
	
	void Start () {
		StartCoroutine(SpawnRock());	//calls the timed enemy spawner function
	}
	
	IEnumerator SpawnRock() {
		//randomises rock rotations within constraints to make sure they shoot upwards
		int randRotateX = Random.Range(-30, 30);
		int randRotateY = Random.Range(-30, 30);
		int randRotateZ = Random.Range(-30, 30);
		yield return new WaitForSeconds(Random.Range(minSpawnTimer, maxSpawnTimer));	//delay between spawning a new enemy
		GameObject rock = (GameObject)Instantiate(eruptionRock, transform.position + new Vector3 (Random.insideUnitSphere.x, 0, Random.insideUnitSphere.z)*spawnWithinRadius, Quaternion.Euler(randRotateX,randRotateY,randRotateZ));		//spawns an enemy
		StartCoroutine(SpawnRock());	//starts the cycle over
		Destroy(rock, 10); //makes sure the rock is destroyed eventually
	}
}
