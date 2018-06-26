using UnityEngine;
using System.Collections;
[RequireComponent (typeof(CharacterController))]

public class Enemy : MonoBehaviour {
	public static float updateDelay;
	public GameObject lootHealth;
	public GameObject lootFuel;
	public bool followPlayer;
	public int health = 3;
	public Color flashColor;
	public float minMoveSpeed = 5f;
	public float maxMoveSpeed = 15f;
	public float detectionDistance = 100;

	private GameObject player;
	private CharacterController controller;
	private Color originalColor;
	private float moveSpeed;
	private float distanceFromTarget;
	private bool iveBeenShot = false;

	void Start () {
		controller = GetComponent<CharacterController> ();	//stores controller
		originalColor = renderer.material.color;	//stores original color
		player = GameObject.FindWithTag ("Player");
		followPlayer = false;
		moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
	}

	void Update () {
		if(health <= 0)
			EnemyDeath();
		StartCoroutine(UpdateDirection());
		if (GameSettings.pauseGame == 0){
			controller.Move (transform.forward * moveSpeed / 10 + transform.up * -0.1f);
			distanceFromTarget = Vector3.Distance (transform.position, player.transform.position);
			if(distanceFromTarget <= detectionDistance || iveBeenShot){
				followPlayer = true;
				updateDelay = 0.2f;
			} else {
				followPlayer = false;
				updateDelay = Random.Range(.5f, 2f);
			}
		} else if(GameSettings.pauseGame == 1)
			controller.Move (transform.forward * 0);
	}

	IEnumerator UpdateDirection(){
		float obstacleDistance;
		if (followPlayer){
			transform.LookAt (player.transform);	//follows the player
			renderer.material.color = flashColor; //enemy flashes colors when shot
			yield return new WaitForSeconds (updateDelay);
		} else {
			renderer.material.color = originalColor; 	//enemy color restores to normal
			RaycastHit hit;
			if(Physics.Raycast(transform.position, transform.forward, out hit, 100)){
				obstacleDistance = hit.distance;
				if (obstacleDistance <= 10){
					transform.Rotate (15, 15, 0);
					yield return new WaitForSeconds (1f);
				}
			}
		}
	}

	void OnHit(int damage){
		health = health - damage; 	//enemy takes damage when shot
		iveBeenShot = true;
	}
	
	void EnemyDeath(){
		player.SendMessage("NotifyDeath");
		float lootType = Random.Range(0f, 1f);
		if((lootType > 0) && (lootType <= 0.1))
			Instantiate(lootHealth, transform.position, Quaternion.identity);
		else if((lootType > 0.1) && (lootType <= 0.5))
			Instantiate(lootFuel, transform.position, Quaternion.identity);
		EnemySpawner.enemyCount--;
		GameSettings.enemiesKilled++;
		Destroy (gameObject);	//enemy dies if health is 0
	}
	
	void Pushback(int explosionForce){
		transform.position -= transform.forward * explosionForce;
	}

	void PushIn(Vector3 implodeTo){
		transform.position = implodeTo;
	}

	void OnCollisionEnter(Collision collision) {
		if(collision.collider.name == "Projectile(Clone)") {
			health--;
		}
	}
}
