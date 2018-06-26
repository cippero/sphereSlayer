using UnityEngine;
using System.Collections;
[RequireComponent (typeof(GUIText))]

public class HUDenemies : MonoBehaviour {
	
	//declares variables
	private GUIText myGUIText;
	
	void Start () {
		myGUIText = GetComponentInChildren<GUIText>();
		if(myGUIText == null)
			Debug.LogError(gameObject.name + ": GUIText is not attached!");
		guiText.material.color = new Color (1f, 1f, 1f, 0.6f);
		guiText.fontSize = 20;
		guiText.pixelOffset = new Vector2(Screen.width-225, -10);
	}

	void Update(){
		if (GameSettings.pauseGame == 1)
			myGUIText.text = null;

	}

	public void UpdateHUDenemies(int enemiesLeft, int enemyCount) {
		
		//displays remaining health
		if(myGUIText != null){
			if (GameSettings.pauseGame == 0)
				myGUIText.text = "Enemies Left: " + enemiesLeft.ToString() + "\nEnemies Detected: " + enemyCount.ToString();
		}
	}

}
