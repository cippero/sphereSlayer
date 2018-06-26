using UnityEngine;
using System.Collections;

public class Vortex : MonoBehaviour {
	
	public float delay = 3;	//time, in seconds, before grenade explodes
	public float radius = 3;	//blast radius
	
	void Start () {
		StartCoroutine(Implode());	//explode following a delay
	}
	
	IEnumerator Implode(){
		yield return new WaitForSeconds(delay); //wait for delay
		Vector3 implodeTo = transform.position;
		//this will return all of the colliders within a certain radius
		Collider[] hits = Physics.OverlapSphere(transform.position, radius);
		//loops through the hits, assigning each in turn to a different variable called 'hit'
		foreach(Collider hit in hits){
			//filter out objects without rigidbodies
			if((hit.rigidbody) && (hit.rigidbody.tag != "Projectile")){
				//apply vortex-style force to each rigidbody
				hit.rigidbody.transform.position = implodeTo;
			} else if(hit.collider.tag == "Enemy"){		//if the collider is an enemy draws it in
				hit.collider.SendMessage("PushIn", implodeTo, SendMessageOptions.DontRequireReceiver);
			}
		}
		Destroy(gameObject); //destroy the grenade
	}
}
