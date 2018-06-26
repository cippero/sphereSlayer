using UnityEngine;
using System.Collections;
[RequireComponent (typeof(CharacterController))]

public class Player : MonoBehaviour {

	public static float teleportTimer = 0;
	public GameObject cameraTarget;
	public Rigidbody projectile1;
	public Rigidbody projectile2;
	public int health = 100;
	public float healthRegenRate = 30f;
	public float moveSpeed = 10f;
	public float rotationSpeed = 5f;
	public float flySpeed = 6.5f;
	public int ammo = 2;
	public float bulletSpeed = 175f;
	public float reloadingTime = 2f;
	public float teleportDistance = 200f;
	public float teleportCooldown = 5f;
	public LayerMask noTeleportThrough;
	public float sprintSpeed = 25;
	public int fuel = 100;
	public float fuelBurnRate = 30f;

	//private variables
	private GameObject gameSettings;
	private GameSettings gameSettingsScript;
	private Vector3 velocity;
	private bool inControl = true;
	private CharacterController controller;
	private float jumpPower = 0f;
	private float maxCamTiltDown = -90f;
	private float maxCamTiltUp = 90f;
	private HUD hudObject;
	private bool isReloading = false;
	private int fullAmmo;
	private int fullHealth;
	private float nextTeleport;
	private float originalSpeed;
	private bool inLava = false;
	private GameObject enemy;
	private GameObject enemyInstance;
	
	void Start(){
		//notifies if a hudGO gameObject isn't attached properly
		GameObject hudGO = GameObject.Find("HUD");
		if(hudGO)
			hudObject = hudGO.GetComponentInChildren<HUD>();
		else
			Debug.LogError("No object named HUD found!");
		if(hudObject)
			hudObject.UpdateHUD(health, ammo, fuel, teleportTimer);
		else
			Debug.LogError("HUD script is not attached to the HUD game object!");

		gameSettings = GameObject.Find("gameSettingsGO");
		gameSettingsScript = gameSettings.GetComponent<GameSettings> ();
		velocity = Vector3.zero;
		cameraTarget.transform.parent = transform;	//camera transform is the child of player
		controller = GetComponent<CharacterController> ();
		Screen.lockCursor = true;	//hides the mouse while playing
		inControl = true; //gives the player controll
		nextTeleport = 0f;

		//stores original values entered as input
		fullAmmo = ammo;
		fullHealth = health;
		originalSpeed = moveSpeed;
		Resume();
		StartCoroutine(HealthRegen());
	}
	
	void Update () {
		if ((Input.GetKeyDown(KeyCode.F1)) && (GameSettings.pauseGame == 0))
			Pause();			//pauses game
		else if ((Input.GetKeyDown(KeyCode.F1)) && (GameSettings.pauseGame == 1))
			Resume();			//resumes game
		CheckHealth();		//checks health to see if player is dead
		if(inControl){	//establishes controls if player is alive
			KeyboardMovement();
			MouseLook();
			ClickToAttack1();
			ClickToAttack2();
			EtcControls();
		}
		velocity -= transform.up * gameSettingsScript.gravityPower * 0.15f;		//applies gravity
		controller.Move(velocity * Time.deltaTime * 40);	//applies movement all at once
	}
	void Pause(){
		Time.timeScale = 0.0f; 		//pauses time
		inControl = false; 		//removes player control
		Screen.lockCursor = false; 		//shows cursor
		GameSettings.pauseGame = 1;
	}
	
	void Resume(){
		Time.timeScale = 1.0f;		//resumes game
		inControl = true;		//restores player control
		Screen.lockCursor = true;		//hides cursor
		GameSettings.pauseGame = 0;
		hudObject.UpdateHUD(health, ammo, fuel, teleportTimer);
	}
	
	void CheckHealth(){
		//checks health, if 0 removes player control
		if (health <= 0) {
			Application.LoadLevel("SphereSlayer_lose");
		}
	}

	//periodically regenerates health
	IEnumerator HealthRegen(){
		health++;
		if(health>fullHealth)
			health=fullHealth;
		hudObject.UpdateHUD(health, ammo, fuel, teleportTimer);
		yield return new WaitForSeconds(healthRegenRate/10);
		StartCoroutine("HealthRegen");
	}
		
	void KeyboardMovement(){
		velocity = Vector3.zero;
		velocity.z = Input.GetAxis("Vertical") * moveSpeed / 10;	//moves forward and back
		velocity.x = Input.GetAxis("Horizontal") * moveSpeed / 10;		//moves sideways

		if (fuel > 0){		//constraints sprinting depending on fuel level
			if (Input.GetKeyDown(KeyCode.LeftShift)){	
				moveSpeed = moveSpeed + sprintSpeed;	//sprint
				StartCoroutine("FuelBurn");
			} else if (Input.GetKeyUp(KeyCode.LeftShift)){		
				moveSpeed = originalSpeed;		//sprint canceled
				StopCoroutine("FuelBurn");
			}

			//flying
			if (Input.GetKey(KeyCode.Space)){
				jumpPower = Mathf.Clamp (jumpPower, jumpPower + flySpeed/100, 1f);		//ascends
				velocity += transform.up * jumpPower;	//applies jump acceleration
			} else if (Input.GetKey(KeyCode.LeftAlt)){
				jumpPower = Mathf.Clamp (jumpPower, jumpPower + flySpeed/100, 1f);		//descends
				velocity -= transform.up * jumpPower;	//applies descent acceleration
			} else {
				jumpPower = 0f;		//stays neutral
			}
		} else {
			moveSpeed = originalSpeed;
		}
		//fuel burn for flying/descending
		if ((Input.GetKeyDown(KeyCode.Space)) || (Input.GetKeyDown(KeyCode.LeftAlt))){
			StartCoroutine("FuelBurn");	
		} else if ((Input.GetKeyUp(KeyCode.Space)) || (Input.GetKeyUp(KeyCode.LeftAlt))){
			StopCoroutine("FuelBurn");
		}
		//teleports forward if not on cooldown
		if (Input.GetKeyDown(KeyCode.F) && Time.time > nextTeleport){		
			Teleport();
			teleportTimer = teleportCooldown;
			StartCoroutine(TimeLeftToTeleport());
		}
		velocity = transform.TransformDirection (velocity);		//moves character in the inputted direction
	}

	void MouseLook(){

		transform.Rotate (0f, (Input.GetAxis ("Mouse X")) * rotationSpeed, 0f);		//camera horizontal rotation
		//camera vertical rotation
		float cameraTilt = Input.GetAxis ("Mouse Y") * -rotationSpeed;
		float proposedRotation = cameraTarget.transform.eulerAngles.x + cameraTilt;
		if ((proposedRotation > maxCamTiltUp) && (proposedRotation < 360f + maxCamTiltDown))
			cameraTilt = 0f;
		cameraTarget.transform.Rotate (cameraTilt, 0f, 0f);
	}

	void EtcControls(){
		if (Input.GetKey(KeyCode.R))		//manually reload
			StartCoroutine(Reload());
	}

	void ClickToAttack1(){
		if(ammo >= 1) {
			if(isReloading == false) {
				if(inControl && Input.GetButtonDown("Fire1")){
					audio.PlayOneShot(gameSettingsScript.pewpew);
					ammo--;		//reduces ammo
					if(hudObject != null)
						hudObject.UpdateHUD(health, ammo, fuel, teleportTimer);	//updates GUI display of health if a HUD object exists
					if(ammo <= 0)
						StartCoroutine(Reload());	//forces reload function if ammo reaches 0
					Rigidbody bullet1 = Instantiate(projectile1, transform.position + transform.forward*5, Quaternion.Euler(0, transform.eulerAngles.y+90, cameraTarget.transform.eulerAngles.x+90)) as Rigidbody;
					bullet1.velocity = Camera.main.transform.forward * bulletSpeed;
					if(Input.GetKey(KeyCode.W))
						bullet1.velocity *= 1.5f;
					if(Input.GetKey(KeyCode.S))
						bullet1.velocity /= 3;
				}
			}
		}
	}

	void ClickToAttack2(){
		if(ammo >= 1) {
			if(isReloading == false) {
				if(inControl && Input.GetButtonDown("Fire2")){
					audio.PlayOneShot(gameSettingsScript.vortex);
					ammo--;		//reduces ammo
					if(hudObject != null)
						hudObject.UpdateHUD(health, ammo, fuel, teleportTimer);	//updates GUI display of health if a HUD object exists
					if(ammo <= 0)
						StartCoroutine(Reload());	//forces reload function if ammo reaches 0
					Rigidbody bullet2 = Instantiate(projectile2, transform.position + transform.forward*5, Quaternion.Euler(0, transform.eulerAngles.y+90, cameraTarget.transform.eulerAngles.x+90)) as Rigidbody;
					bullet2.velocity = Camera.main.transform.forward * bulletSpeed;
					if(Input.GetKey(KeyCode.W))
						bullet2.velocity *= 1.5f;
					if(Input.GetKey(KeyCode.S))
						bullet2.velocity /= 3;
				}
			}
		}
	}

	IEnumerator Reload() {
		isReloading = true;		//makes player unable to fire while reloading
		yield return new WaitForSeconds(reloadingTime);
		ammo = fullAmmo;	//restores full ammo
		hudObject.UpdateHUD(health, ammo, fuel, teleportTimer);
		isReloading = false;
	}

	public IEnumerator DamagePerTime(float delay){
		health--;
		hudObject.UpdateHUD(health, ammo, fuel, teleportTimer);
		yield return new WaitForSeconds(delay);
		if (GameSettings.inDamage == true){
			StartCoroutine(DamagePerTime(delay));
		}
	}

	IEnumerator DamagePerTimeLava(float delay){
		health--;
		hudObject.UpdateHUD(health, ammo, fuel, teleportTimer);
		yield return new WaitForSeconds(delay);
		if (inLava == true){
			StartCoroutine(DamagePerTimeLava(delay));
		}
	}

	void Teleport(){
		nextTeleport = Time.time + teleportCooldown;	//sets the cooldown for teleporting
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));	//casts a ray through the middle of the screen
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, noTeleportThrough)){
			if (teleportDistance >= hit.distance){
				transform.position += transform.forward * hit.distance;
				transform.position -= transform.forward * 5f;
			}else{
				transform.position += transform.forward * teleportDistance;		//sets the distance for teleporting
			}
		}
	}

	IEnumerator TimeLeftToTeleport(){
		yield return new WaitForSeconds(1f);
		teleportTimer--;
		hudObject.UpdateHUD(health, ammo, fuel, teleportTimer);
		if(teleportTimer > 0)
			StartCoroutine(TimeLeftToTeleport());
	}

	IEnumerator FuelBurn(){
		if (fuel > 0){
			fuel--;
			hudObject.UpdateHUD(health, ammo, fuel, teleportTimer);
			yield return new WaitForSeconds(fuelBurnRate/100);
			StartCoroutine("FuelBurn");
		}
		else 
			StopCoroutine("FuelBurn");
	}

	void PushBack(int pushDistance){
		print (this + " was pushed back!");
		transform.position -= transform.forward * pushDistance;
	}
	
	void NotifyDeath(){
		GameSettings.inDamage = false;
		StopCoroutine(DamagePerTime(2f));
	}

	void OnTriggerEnter(Collider other) {
		//hurts the player when touching a damaging object
		if (other.tag == "Enemy"){
			GameSettings.inDamage = true;
			StartCoroutine(DamagePerTime(2f));
		}
		//hurts the player when touching a damaging object
		if (other.tag == "Lava"){
			inLava = true;
			StartCoroutine(DamagePerTimeLava(0.5f));
		}
		if (other.tag == "Damage"){
			health -= 10;
		}
		if (other.tag == "Objective"){
			Application.LoadLevel("SphereSlayer_win");
		}
		if(hudObject != null)
			hudObject.UpdateHUD(health, ammo, fuel, teleportTimer);	//updates GUI display if a HUD object exists
	}

	void OnTriggerExit(Collider other){
		//removes damage per time if player exists the enemy collider
		if (other.tag == "Enemy"){
			GameSettings.inDamage = false;
			StopCoroutine(DamagePerTime(2f));
		}
		//removes damage per time if player exists the damaging collider
		if (other.tag == "Lava"){
			inLava = false;
		}
	}
}