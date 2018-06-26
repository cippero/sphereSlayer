using UnityEngine;
using System.Collections;
[RequireComponent (typeof(GUIText))]

public class WinScreen : MonoBehaviour {
	private GUIText myGUIText;
	
	void Start () {
		myGUIText = GetComponentInChildren<GUIText>();
		if(myGUIText == null)
			Debug.LogError(gameObject.name + ": GUIText is not attached!");
		guiText.material.color = new Color (1f, 1f, 1f, 0.6f);
		guiText.fontSize = 20;
		guiText.pixelOffset = new Vector2(Screen.width/2-235, -Screen.height/2+100);
		guiText.alignment = TextAlignment.Center;
		myGUIText.text = "You Win The Game!!! \nAll Hail The Mighty Sphere-Slayer! \n\n\n\n\n\n\n\n\n You managed to bring back " + GameSettings.fuelCollected.ToString() + " resources! \n You've also killed " + GameSettings.enemiesKilled.ToString() + " enemies. Good.";
		
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