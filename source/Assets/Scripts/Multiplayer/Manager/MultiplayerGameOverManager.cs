/* Script to control the attacking behaviour of the player

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class MultiplayerGameOverManager : MonoBehaviour
{
	// trigger to tell if game has started
	bool gameStart = false;
	// trigger to tell if game has ended
	bool gameOver;

	// Health sliders assigned to Players 1 and 2
	public Slider play1;
	public Slider play2;

	// players to reference scores
	public GameObject player1;
	public GameObject player2;

	// winner/loser text
	public Text p1result;
	public Text p2result;

	// score text
	public Text p1score;
	public Text p2score;

	// title and instructions
	public Text title;
	public Text instruction;

	// game over background
	public Image resultPanel;

	// player's health
	float health1;
	float health2;

	// player's score
	int tmpscore1;
	int tmpscore2;

	// timer to track time before automatic 
	// transition from Multiplayer to Main Menu
	float timer;

	
	void Awake()
	{
		// set player's health to initial value of the health bar
		health1 = play1.value;
		health2 = play2.value;
		timer = 0.0f;
		gameOver = false;
	}
	/*
	void Update()
	{
		// Find all player GameObjects
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		if(players.Length < 2){
			return;
		}
		
		if(gameOver)
		{
			if(timer < Time.realtimeSinceStartup)
			{
				Application.LoadLevel(0);
			}
			
			return;
			
			
		}
		if(!this.gameStart && Network.connections.Length > 0)
		{
			
			// Server is Player 1; Client is Player 2
			if(Network.isServer)
			{
				player1 = players[0];
				player2 = players[1];
			}
			else
			{
				player1 = players[1];
				player2 = players[0];
			}
			this.gameStart = true;
		}
		
		if(this.gameStart)
		{
			health1 = player1.GetComponent<MultiplayerPlayerHealth>().currentHealth;
			health2 = player2.GetComponent<MultiplayerPlayerHealth>().currentHealth;
			Debug.Log (health1 + " " + health2);
			if (health1 <= 0 || health2 <= 0)
			{				
				Time.timeScale = 0.0001f;
				this.CompareScore();
				timer = Time.realtimeSinceStartup + 0.001f;
				gameOver = true;
			}
		}
	}*/

	void Update()
	{
		// Find all player GameObjects
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		// Don't do anything if client is not connected to the game
		if(players.Length < 2){
			return;
		}

		// If the game has ended, wait for 5 seconds to transit from
		// Multiplayer to Main Menu
		if(gameOver)
		{
			timer -= Time.deltaTime;
			Debug.Log(timer);
			if(timer < 0)
			{
				Application.LoadLevel(0);
			}
			
			return;	
		}

		if(!this.gameStart && Network.connections.Length > 0)
		{
			
			// Server is Player 1; Client is Player 2
			if(Network.isServer)
			{
				player1 = players[0];
				player2 = players[1];
			}
			else
			{
				player1 = players[1];
				player2 = players[0];
			}
			this.gameStart = true;
		}
		
		if(this.gameStart)
		{
			// Check if any of the player has its health <= zero
			health1 = player1.GetComponent<MultiplayerPlayerHealth>().currentHealth;
			health2 = player2.GetComponent<MultiplayerPlayerHealth>().currentHealth;
			Debug.Log (health1 + " " + health2);
			if (health1 <= 0 || health2 <= 0)
			{		
				// Destroy all enemy objects
				if(Network.isServer){
					//Time.timeScale = 0.0f;
					GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
					foreach (GameObject enemy in enemies){
						Network.Destroy(enemy);
					}
				}

				// Compare players' scores
				this.CompareScore();
				timer = 5;
				gameOver = true;
			}
		}
	}
	
	void OnLevelWasLoaded(int level)
	{
		if(level == 0){
			if(Network.isServer){
				Network.Destroy (player1);
				Network.Destroy (player2);
				MasterServer.UnregisterHost();
			}
		}
	}
	
	void CompareScore()
	{
		//display panel
		//resultPanel.color = new Color(0,234,255,1.0f) ;

		//display title and instructions
		title.text = "Game Over";
		//instruction.text = "Please wait while we bring you back to main menu...";

		tmpscore1 = player1.GetComponent<MultiplayerPlayerScore> ().score;
		tmpscore2 = player2.GetComponent<MultiplayerPlayerScore> ().score;
		
		//Display scores
		p1score.text = "Score: " + tmpscore1.ToString();
		p2score.text = "Score: " + tmpscore2.ToString();

		//Check high score
		//this.StoreScore ();
		/*
		//Evaluate winner
		if(tmpscore1 > tmpscore2)
		{
			p1result.text = "P1 Win";
			p2result.text = "P2 Lose";
		}
		else if (tmpscore1 < tmpscore2)
		{
			p1result.text = "P1 Lose";
			p2result.text = "P2 Win";
		}
		else
		{
			p1result.text = "P1 Draw";
			p2result.text = "P2 Draw";
		}*/

	}

	void StoreScore()
	{
		System.IO.StreamReader scoreFile;
		System.IO.StreamWriter scoreMaker;
		int currHighestScore;
		int tmpScore;
		//Text text;
		Text HighestScore;

		string filePath = Application.persistentDataPath + "/score.txt";

		// Open up file to store score and retrieve highest score
		if (!System.IO.File.Exists (filePath)) 
		{
			Debug.Log ("Create file!");
			scoreMaker = System.IO.File.CreateText (filePath);
			scoreMaker.Close();
			currHighestScore = 0;
		} 
		else 
		{
			Debug.Log ("Read file!");
			scoreFile = new System.IO.StreamReader (filePath);
			currHighestScore = int.Parse (scoreFile.ReadLine ());
			scoreFile.Close();
		}

		// Retrieve respective player's score for this round of game
		if(Network.isServer)
		{
			tmpScore = tmpscore1;
		}
		else
		{
			tmpScore = tmpscore2;
		}



		try{
			if (tmpScore > currHighestScore) 
			{
				currHighestScore = tmpScore;		
				System.IO.File.WriteAllText(filePath, currHighestScore.ToString());
			}
		}
		catch(System.Exception e)
		{
			HighestScore = GameObject.FindGameObjectWithTag ("HighestScoreText").GetComponent<Text>();
			HighestScore.text = currHighestScore.ToString();
		}
	}
}