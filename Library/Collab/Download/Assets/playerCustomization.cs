using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCustomization : MonoBehaviour {

	public static int favColor;
	public GUISkin customSkin;

	private Canvas menuCanvas;

	public void Start() {
		menuCanvas = transform.parent.gameObject.GetComponent<Canvas>();
	}

	public void Update() {
		
	}
	
	public void OnGUI(){
		GUI.Label(new Rect(400,400,100,100),PlayerPrefs.GetString("favColor"));
	}
}
