/* Script to control the attacking behaviour of the player

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseManager : MonoBehaviour {
	private bool Pause = false;
	private bool Quit = false;

	// Storing the pause button
	GameObject pauseButton;

	// Storing the menu on the pause mode
	GameObject[] pauseMenuButton;

	// Storing the menu when the quit button is clicked
	GameObject[] quit;

	void Awake()
	{
		pauseButton = GameObject.FindWithTag("pauseButton");
		pauseMenuButton = GameObject.FindGameObjectsWithTag("playButton");
		quit = GameObject.FindGameObjectsWithTag("quitButton");

	}

	/**
	 * @void, set the pauseButton to appear on play mode, and gone in pause mode
	 **/
	private void setPause(bool cond)
	{
		pauseButton.SetActive (cond);
	}

	/**
	 * @void, set the pause menu to appear 
	 **/
	private void setPauseMenu(bool cond)
	{
		foreach(GameObject obj in pauseMenuButton)
		{
			obj.SetActive(cond);
		}
	}

	/**
	 * @void, set the quit menu to appear
	 **/
	private void setQuitMenu(bool cond)
	{
		foreach(GameObject obj in quit)
		{
			obj.SetActive(cond);
		}
	}

	void FixedUpdate()
	{
		setPause (!Pause);
		setPauseMenu (Pause);
		setQuitMenu (Quit);
	}

	/**
	 * @void, entering the pause mode
	 **/
	public void pauseGame()
	{
		if (!Pause) 
		{
			Pause = true; 
			Time.timeScale = 0.0f;

		} 
		else 
		{
			Pause = false; 
			Time.timeScale = 1.0f;
		}
		setPause (!Pause);
		setPauseMenu (Pause);


	}

	/**
	 * @void, entering in quit mode 
	 **/
	public void quitGame()
	{
		Quit = true;
		setPauseMenu (!Quit);
		setQuitMenu (Quit);
	}

	/**
	 * @void, back to menu when the yes button is clicked on quit mode
	 **/
	public void yesButton()
	{
		pauseGame ();
		Application.LoadLevel (0);
	}

	/**
	 * @void, back to play mode when the no button is clicked on quit mode
	 **/
	public void noButton()
	{
		Quit = false;
		setQuitMenu (Quit);
		pauseGame ();
	}
}
