using UnityEngine;
using System.Collections;
[RequireComponent (typeof(GUIText))]

public class Quote : MonoBehaviour {
	
	private GUIText myGUIText;
	
	void Start () {
		myGUIText = GetComponentInChildren<GUIText>();
		if(myGUIText == null)
			Debug.LogError(gameObject.name + ": GUIText is not attached!");
		guiText.material.color = new Color (1f, 1f, 1f, 0.4f);
		guiText.fontSize = 18;
		guiText.pixelOffset = new Vector2(Screen.width/2-330, -Screen.height/2-120);
		guiText.alignment = TextAlignment.Center;
		guiText.fontStyle = FontStyle.Italic;
		myGUIText.text = "''Don't feel bad about annihilating the sphere population. \nOnce you get to know them, they're actually mostly pretty evil. \n(Obviously. I mean, just look at that sinister-looking glow) \nNow it's those helpless cubical flying rocks that you should feel bad about.''";
	}
}