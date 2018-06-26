using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour {

	public Transform target;

	void LateUpdate(){
		transform.position = new Vector3 (target.position.x, target.position.y, target.position.z);
	}
}
