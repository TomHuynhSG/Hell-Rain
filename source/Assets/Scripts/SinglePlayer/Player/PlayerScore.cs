/* Script to control the score of the player

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {
	public Text scorestring;
	public float score;

	// Use this for initialization
	void Start () {
		this.score = 0;
		this.SetScoreString ();
	}
	
	// Update is called once per frame
	void Update () {
		this.score += Time.deltaTime;
		this.SetScoreString ();
	}

	// Displays score onto canvas
	void SetScoreString(){
		this.scorestring.text = "Score: " + this.score.ToString ("0");
	}
}
