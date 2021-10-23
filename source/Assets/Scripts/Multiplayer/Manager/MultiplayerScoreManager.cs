/* Script to control the attacking behaviour of the player

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultiplayerScoreManager : MonoBehaviour
{
	// Player for health slider to take reference (health)
	public GameObject player;
	// Index of player (Server->0 ; Client->1)
	public int playerIndex;
	// Player's Name
	public string playerName;
	// Score text
	public Text scoreText;
	// Trigger to update player's score
	private bool gameStart = false;
	
	// Use this for initialization
	void Start () 
	{	
		scoreText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {		
		// Assign health bar to each player when client has joined
		if(!gameStart && Network.connections.Length > 0)
		{
			// Find all player GameObjects
			GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

			// Server is Player 1; Client is Player 2
			if(Network.isServer)
			{
				player = players[playerIndex % 2];
			} else {
				int tmpindex = (playerIndex + 1) % 2;
				Debug.Log("testScoreRefClient - " + "playerIndex: " + playerIndex.ToString() + " Result: " + tmpindex.ToString());
				player = players[tmpindex];
			}


			// Don't assign health bar any more after game starts
			gameStart = true;
		}
		
		// Change health slider value only after game starts
		if(gameStart)
		{
			int tmpscore = player.GetComponent<MultiplayerPlayerScore> ().score;
			scoreText.text = playerName + ": " + tmpscore;
		}
	}
    /*public static float score;


    Text text;


    void Awake ()
    {
        text = GetComponent <Text> ();
        score = 0;
    }


    void Update ()
    {
		// Increase score with time
		score += Time.deltaTime;
		// Display score text
        text.text = "P1 Score: " + (int)score;

		// update scores only if 1 other player is connected
		if(Network.connections.Length > 0)
		{
			score += Time.deltaTime;
	        text.text = "P1 Score: " + (int)score;
		}
    }*/
}
