using UnityEngine;
using System.Collections;
[RequireComponent (typeof(GUIText))]

public class Credits : MonoBehaviour {
	
	private GUIText myGUIText;
	
	void Start () {
		myGUIText = GetComponentInChildren<GUIText>();
		if(myGUIText == null)
			Debug.LogError(gameObject.name + ": GUIText is not attached!");
		guiText.material.color = new Color (1f, 1f, 1f, 0.4f);
		guiText.fontSize = 14;
		guiText.pixelOffset = new Vector2(Screen.width/2-470, -Screen.height/2-210);
		guiText.alignment = TextAlignment.Center;
		myGUIText.text = "                                                                                                                                                           Quote by a sketchy-looking old sage. \n\nGame Made by Gil Weinstock \nAll art and assets are placeholders \n(Especially the sound effects)";
	}
}