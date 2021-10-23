/* Script to control the attacking behaviour of the player

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */


using UnityEngine;
using System.Collections;

public class PositionScale : MonoBehaviour {

	
	public Vector2 defaultResolution = new Vector2 (906f,423f);
	private Vector2 deviceResolution ;
	private Vector2 ratioResolution;
	private Vector3 startPosition;
	private RectTransform rectTrans;
	
	// Use this for initialization
	void Start () {
		rectTrans=  GetComponent<RectTransform>();
		startPosition = rectTrans.localPosition;

		
	}
	
	// Update is called once per frame
	void Update () {
		SetScale ();
	}
	
	void SetScale(){
		deviceResolution = new Vector2 (Screen.width,Screen.height);
		ratioResolution = new Vector2 (deviceResolution.x/defaultResolution.x, deviceResolution.y/defaultResolution.y);
		rectTrans.localPosition = new Vector3(startPosition.x*ratioResolution.x, startPosition.y, startPosition.z);

	}
}
