using UnityEngine;
using System.Collections;
[RequireComponent (typeof(GUIText))]

public class LoseScreen : MonoBehaviour {
	private GUIText myGUIText;
	
	void Start () {
		myGUIText = GetComponentInChildren<GUIText>();
		if(myGUIText == null)
			Debug.LogError(gameObject.name + ": GUIText is not attached!");
		guiText.material.color = new Color (1f, 1f, 1f, 0.6f);
		guiText.fontSize = 20;
		guiText.pixelOffset = new Vector2(Screen.width/2-280, -Screen.height/2+70);
		guiText.alignment = TextAlignment.Center;
		myGUIText.text = "You Are Dead. :( \n\n\n\n\n\n\n\n\n You aaaaalmost managed to bring back " + GameSettings.fuelCollected.ToString() + " resources. \n You've also killed " + GameSettings.enemiesKilled.ToString() + " enemies. Meh.";
		
		Screen.lockCursor = false;
	}
	
	void OnGUI() {
		// button to restart the current level
		if(GUI.Button(new Rect(Screen.width*0.5f-145, Screen.height*0.5f, 200, 40),"Restart")){
			GameSettings.enemiesKilled = 0;
			EnemySpawner.enemyCount = 0;
			Player.teleportTimer = 0;
			Application.LoadLevel("SphereSlayer_game");
		}
		if(GUI.Button(new Rect(Screen.width*0.5f-145, Screen.height*0.5f+50f, 200, 40),"Quit"))
			Application.Quit();
	}
}