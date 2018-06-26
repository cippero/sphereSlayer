using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {
	public static int pauseGame = 0;
	public static bool inDamage = false;
	public static int playerMaxFuel;
	public static int enemiesKilled = 0;
	public static int maxEnemies;
	public static int fuelCollected;
	
	public float gravityPower = 9;
	public float floorScrollSpeed = 4f;
	public int maxEnemiesOnScreen = 150;
	public Renderer rend;
	public AudioClip pewpew;
	public AudioClip vortex;
	
	private Player playerScript;
	
	void Start() {
		playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
		rend = GameObject.Find("Floor").GetComponent<Renderer>();
		maxEnemies = maxEnemiesOnScreen;
		playerMaxFuel = playerScript.fuel*5;
	}
	
	void Update() {
		fuelCollected = playerScript.fuel;
		float offset = Time.time * floorScrollSpeed/200;
		rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
		rend.material.SetTextureOffset("_Illum", new Vector2(offset, 0));
	}
}
