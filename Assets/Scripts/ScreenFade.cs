using UnityEngine;
using System.Collections;
[RequireComponent (typeof(GUITexture))]

public class ScreenFade : MonoBehaviour {

	public float fadeSpeed = 1.5f;

	private GUITexture myGUITexture;
	
	void Start (){
		myGUITexture = GetComponentInChildren<GUITexture>();
		if(myGUITexture == null)
			Debug.LogError(gameObject.name + ": GUITexture is not attached!");
		guiTexture.pixelInset = new Rect(-Screen.width/2, -Screen.height/2, Screen.width, Screen.height);
		guiTexture.color = Color.black;
	}
	
	void Update (){
		if(GameSettings.pauseGame == 0)
			FadeToClear();
		if(GameSettings.pauseGame == 1)
			GoToBlack();
	}
	
	void FadeToClear (){
		guiTexture.color = Color.Lerp(guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
		if(guiTexture.color.a <= 0.05f){
			guiTexture.color = Color.clear;
			guiTexture.enabled = false;
		}
	}
	
	
	void GoToBlack (){
		guiTexture.enabled = true;
		guiTexture.color = new Color (0f, 0f, 0f, 0.4f);
	}
}