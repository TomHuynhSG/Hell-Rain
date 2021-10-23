/* Script to control the attacking behaviour of the player

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */


using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class FontScale : MonoBehaviour {

	public Vector2 defaultResolution = new Vector2 (906f,423f);
	private Vector2 deviceResolution ;
	private Vector2 ratioResolution;
	private int startFontSize;
	private Text text;


	// Use this for initialization
	void Start () {
		text=GetComponent<Text>();
		startFontSize = text.fontSize;
	

	}
	
	// Update is called once per frame
	void Update () {
		SetScale ();
	}

	void SetScale(){
		deviceResolution = new Vector2 (Screen.width,Screen.height);
		ratioResolution = new Vector2 (deviceResolution.x/defaultResolution.x, deviceResolution.y/defaultResolution.y);
		//Debug.Log ("Ratio X " +ratioResolution.x);
		text.fontSize = (int)(startFontSize*ratioResolution.x) ;
	}
}
