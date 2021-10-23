/* Script to control the attacking behaviour of the player

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultiplayerHealthUIManager : MonoBehaviour {
	// Player for health slider to take reference (health)
	public GameObject player;
	// Health slider (Component of Parent)
	public Slider healthSlider;
	// Index of player (Server->0 ; Client->1)
	public int playerIndex;
	// Trigger to assign health bar to each player
	private bool gameStart = false;
	
	// Use this for initialization
	void Start () 
	{	
		healthSlider = GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {


		// Assign health bar to each player when client has joined
		if(!this.gameStart && Network.connections.Length > 0)
		{
			// Find all player GameObjects
			GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

			// Server is Player 1; Client is Player 2
			if(Network.isServer)
			{
				player = players[playerIndex % 2];
			} else {
				player = players[(playerIndex + 1) % 2];
			}

			// Don't assign health bar any more after game starts
			this.gameStart = true;
		}

		// Change health slider value only after game starts
		if(this.gameStart)
		{
			healthSlider.value = player.GetComponent<MultiplayerPlayerHealth> ().currentHealth;
		}
	}
}
