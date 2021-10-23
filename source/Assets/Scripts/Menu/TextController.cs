/* Script to control the attacking behaviour of the player

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */


using UnityEngine;
using System.Collections;

public class TextController : MonoBehaviour {

	/*
	 * @void, starting the single player mode 
	 */ 
	public void StartSinglePlayer() 
	{
		//guiText.color = Color.red;
		Application.LoadLevel (1);
	}

	/*
	 * @void, show the tutorial (how to play) on the menu by using the animation on the camera
	 */ 
	public void howToPlay()
	{
		GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera");
		camera.animation["HowToPlay"].speed = 1.0f;
		camera.animation.Play ("HowToPlay");
	}

	/*
	 * @void, show the main part of the menu by using the animation on the camera
	 */
	public void backToMenu()
	{
		GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera");
		camera.animation["HowToPlay"].speed = -1.0f;
		camera.animation["HowToPlay"].time = camera.animation["HowToPlay"].length;
		camera.animation.Play ("HowToPlay");
	}

	/*
	 * @void, show the highest score on the menu by using the animation on the camera
	 */
	public void highestScore()
	{
		GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera");
		camera.animation ["HighestScore"].speed = 1.0f;
		camera.animation.Play ("HighestScore");
	}

	/*
	 * @void, show the main part of the menu from the highest score menu by using the animation on the camera
	 */
	public void backFromScore()
	{
		GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera");
		camera.animation ["HighestScore"].speed = -1.0f;
		camera.animation["HighestScore"].time = camera.animation["HighestScore"].length;
		camera.animation.Play ("HighestScore");
	}
}
