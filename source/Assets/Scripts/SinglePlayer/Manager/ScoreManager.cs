/* Script to control the attacking behaviour of the player

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class ScoreManager : MonoBehaviour
{
    public static int score;
	private static int currHighestScore;
	private static int currGamePlayed;
	System.IO.StreamReader scoreFile;
	System.IO.StreamWriter scoreMaker;
    Text scoreText;
	Text HighestScore;


	// The file path for storing the score on .txt on Android
	private string filePath = Application.persistentDataPath + "/score.txt";
	// The file path for storing the score on .txt on PC
	//private string filePath = "Assets/score.txt";


    void Awake ()
    {
		// Open up file to store score and retrieve highest score
		try 
		{
			// Create the file if the file does not exist
			if (!System.IO.File.Exists (filePath)) 
			{
				Debug.Log ("Create file!");
				scoreMaker = System.IO.File.CreateText (filePath);
				scoreMaker.Close();
				currHighestScore = 0;
			} 

			// Retrive the highest score from the file
			else 
			{
				Debug.Log ("Read file!");
				scoreFile = new System.IO.StreamReader (filePath);
				currHighestScore = int.Parse (scoreFile.ReadLine ());
				scoreFile.Close();
			}
		}
		catch (System.Exception e)
		{
			// Do nothing
				
		}

		// initialise score
		score = 0;
	
		// Works for updating the score on the single player mode 
		try 
		{
			scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
			scoreText.text = score.ToString();
		}

		// Works for showing the highest score from the file to the menu in menu mode
		catch(System.Exception e)
		{
			HighestScore = GameObject.FindGameObjectWithTag ("HighestScoreText").GetComponent<Text>();
			HighestScore.text = currHighestScore.ToString();
		}
    }


	void Update ()
    {
		// Updates highest score after end of game
		try 
		{

			scoreText.text = "Score: " + score;
			if (score > currHighestScore) 
			{
				currHighestScore = score;
				try 
				{

					System.IO.File.WriteAllText(filePath, currHighestScore.ToString());
			
				}
				catch (System.Exception e)
				{
					//Do nothing
				}
			}
		}

		// The exception will be catch if the game is in the menu mode
		catch(System.Exception e)
		{
			HighestScore.text = currHighestScore.ToString();
		}

    }
}
