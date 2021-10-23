/* Script to control the attacking behaviour of the player

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */


using UnityEngine;
using System.Collections;

public class UIScale : MonoBehaviour {

	public Vector2 defaultResolution = new Vector2 (906f,423f);
	private Vector2 deviceResolution ;
	private Vector2 ratioResolution;

	private RectTransform rectTrans;

	// Use this for initialization
	void Start () {
		rectTrans=  GetComponent<RectTransform>();

	}
	
	// Update is called once per frame
	void Update () {
		SetScale ();
	}

	void SetScale(){
		deviceResolution = new Vector2 (Screen.width,Screen.height);
		ratioResolution = new Vector2 (deviceResolution.x/defaultResolution.x, deviceResolution.y/defaultResolution.y);
		
		rectTrans.localScale = new Vector3(ratioResolution.x,ratioResolution.y, 0);
	}
}
