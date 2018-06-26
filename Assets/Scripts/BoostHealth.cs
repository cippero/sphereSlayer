using UnityEngine;
using System.Collections;

public class BoostHealth : MonoBehaviour {

	public int minAmount = 5;
	public int maxAmount = 30;
	public Color boostColor;

	private GameObject player;
	private Player playerScript;
	private int fullHealth;
	private float randomRange;
	private Light lightGlow;
	private int numAmount;
	private bool isHealth;

	void Start () {
		player = GameObject.FindWithTag ("Player");
		playerScript = player.GetComponent<Player> ();	//stores player script
		fullHealth = playerScript.health;
		numAmount = Random.Range (minAmount, maxAmount);
		foreach (Transform child in transform)
		{
			lightGlow = child.light;
		}
		renderer.material.color = boostColor;
		lightGlow.color = Color.green;
		Destroy(gameObject, 5);
	}
	
	void OnTriggerEnter(Collider other){
		//interacts with player if touches him
		if(other.name == "Player"){
			if (playerScript.health + numAmount >= fullHealth)
				playerScript.health = fullHealth;
			else
				playerScript.health += numAmount;
			Destroy (gameObject);
		}
	}
	
}
