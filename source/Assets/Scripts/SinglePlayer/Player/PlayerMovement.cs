/* Script to control the movement of the player

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */

using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speed = 6f;            // The speed that the player will move at.
	
	
	Vector3 movement;                   // The vector to store the direction of the player's movement.
	Animator anim;                      // Reference to the animator component.
	Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
	int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
	public float turnSmoothing = 15f;   // A smoothing value for turning the player.
	public bool reverseDirection;

	//AudioSource AttackAudio;

	
	void Awake ()
	{
			
		// Set up references.
		reverseDirection = false;
		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
	}
	
	void FixedUpdate ()
	{
		// Store the input axes. ( for PC and MAC )
		//float h = Input.GetAxis("Horizontal");
		//float v = Input.GetAxis("Vertical");

		// Store the input axes. ( for Android )
		float h = Input.acceleration.x;
		float v = Input.acceleration.y+0.6f;

		// Move the player around the scene.

		if(h != 0f || v != 0f)
		{
			if (reverseDirection == false) {
				Move (h, v);
				TurningWithAccelerometer(h, v);
			} else {
				Move (-h, -v);
				TurningWithAccelerometer(-h, -v);
			}



		}

		// Animate the player.
		Animating (h, v);
	}
	
	
	void Move (float h, float v)
	{
		// Set the movement vector based on the axis input.

		movement.Set (h, -0.1f, v);
		
		// Normalise the movement vector and make it proportional to the speed per second.
		movement = movement.normalized * speed * Time.deltaTime;
		
		// Move the player to it's current position plus the movement.
		playerRigidbody.MovePosition (transform.position + movement);
	}
	



	void TurningWithAccelerometer (float horizontal, float vertical)
	{
		// Create a new vector of the horizontal and vertical inputs.
		Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
		
		// Create a rotation based on this new vector assuming that up is the global y axis.
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		
		// Create a rotation that is an increment closer to the target rotation from the player's rotation.
		Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, targetRotation, turnSmoothing * Time.deltaTime);
		
		// Change the players rotation to this new rotation.
		rigidbody.MoveRotation(newRotation);
	}

	
	void Animating (float h, float v)
	{
		// Create a boolean that is true if either of the input axes is non-zero.
		bool running = h != 0f || v != 0f;
		
		// Tell the animator whether or not the player is walking.
		anim.SetBool ("IsRunning", running);
	}
}
