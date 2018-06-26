using UnityEngine;
using System.Collections;
[RequireComponent (typeof(GUIText))]

public class HUD : MonoBehaviour {
	private GUIText myGUIText;
	private Player playerScript;

	void Start () {
		myGUIText = GetComponentInChildren<GUIText>();
		if(myGUIText == null)
			Debug.LogError(gameObject.name + ": GUIText is not attached!");
		guiText.material.color = new Color (1f, 1f, 1f, 0.6f);
		guiText.fontSize = 20;
		playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
		myGUIText.text = "Health: " + playerScript.health.ToString() + "\nAmmo: " + playerScript.ammo.ToString() + "\nFuel: " + playerScript.fuel.ToString() + "/" + GameSettings.playerMaxFuel.ToString() + "\nTeleport: Ready";
	}

	void Update(){
		if (GameSettings.pauseGame == 1){
			guiText.pixelOffset = new Vector2(Screen.width/2-360, -Screen.height/2+200);
			guiText.alignment = TextAlignment.Center;
			myGUIText.text = "Collect as many resources as possible from fallen spheres \nand make it to the exit portal... with your life! \n\n\nWASD to move   |   Hold space to fly   |   Shift to boost   |   F to teleport \nLeft mouse to fire   |   Right mouse to pull   |   R to reload \n\nF1 to play/pause";
		} else {
			guiText.pixelOffset = new Vector2(20, -10);
			guiText.alignment = TextAlignment.Left;
		}
	}

	public void UpdateHUD(int health, int ammo, int fuel, float TeleportTimer) {
		string teleportCountdown = TeleportTimer.ToString() + "s";
		if (TeleportTimer == 0)
			teleportCountdown = "Ready";
		if(myGUIText != null)
			if (GameSettings.pauseGame == 0)
				myGUIText.text = "Health: " + health.ToString() + "\nAmmo: " + ammo.ToString() + "\nFuel: " + fuel.ToString() + "/" + GameSettings.playerMaxFuel.ToString() + "\nTeleport: " + teleportCountdown;
	}

	void OnGUI() 
		
	{
		
		// button to restart the current level
		if(GameSettings.pauseGame == 1){
			if(GUI.Button(new Rect(Screen.width*0.5f-145, Screen.height*0.5f, 200, 40),"Restart")){
				GameSettings.enemiesKilled = 0;
				Player.teleportTimer = 0;
				EnemySpawner.enemyCount = 0;
				Application.LoadLevel("SphereSlayer_game");
			}
			if(GUI.Button(new Rect(Screen.width*0.5f-145, Screen.height*0.5f+50f, 200, 40),"Quit"))
				Application.Quit();
		}
	}
}
