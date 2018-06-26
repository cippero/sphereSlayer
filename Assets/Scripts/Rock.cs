using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {

	public float thrust = 200f;
	public float thrustDuration = 1f;
	public GameObject loot;

	private Rigidbody rb;
	private float thrustEnd;
	private float randThrust;

	void Start() {
		rb = GetComponent<Rigidbody>();
		thrustEnd = Time.time + thrustDuration;
		float minThrust = thrust*0.75f;
		float maxThrust = thrust*1.25f;
		randThrust = Random.Range(minThrust,maxThrust);
	}
	void FixedUpdate() {
		if (Time.time < thrustEnd)
			rb.AddForce(transform.up * randThrust*10);
	}
	
	void OnTriggerEnter(Collider other){
		if (other.tag == "Lava"){
			float lootChance = Random.Range(0f,1f);
			if(lootChance <= 0.1)
				Instantiate(loot, transform.position + new Vector3 (0, 10, 0), Quaternion.Euler(0,0,0));
			Destroy(gameObject);
		}
	}
}