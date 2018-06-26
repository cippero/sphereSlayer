using UnityEngine;
using System.Collections;
[RequireComponent (typeof(GUIText))]

public class StartScreen : MonoBehaviour {
	
	private GUIText myGUIText;
	
	void Start () {
		myGUIText = GetComponentInChildren<GUIText>();
		if(myGUIText == null)
			Debug.LogError(gameObject.name + ": GUIText is not attached!");
		guiText.material.color = new Color (1f, 1f, 1f, 0.6f);
		guiText.fontSize = 20;
		guiText.pixelOffset = new Vector2(Screen.width/2-360, -Screen.height/2+200);
		guiText.alignment = TextAlignment.Center;
		myGUIText.text = "Collect as many resources as possible from fallen spheres \nand make it to the exit portal... with your life! \n\n\nWASD to move   |   Hold space to fly   |   Shift to boost   |   F to teleport \nLeft mouse to fire   |   Right mouse to pull   |   R to reload \n\nF1 to play/pause";
	}
	
	void OnGUI(){
		// button to restart the current level
			if(GUI.Button(new Rect(Screen.width*0.5f-145, Screen.height*0.5f, 200, 40),"Start"))
				Application.LoadLevel("SphereSlayer_game");
			if(GUI.Button(new Rect(Screen.width*0.5f-145, Screen.height*0.5f+50f, 200, 40),"Quit"))
				Application.Quit();
	}
}
