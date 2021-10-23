/* Script to control the movement of the player

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */

using UnityEngine;
using System.Collections;

public class MultiplayerPlayerMovement : MonoBehaviour {

	public float speed = 6f;            // The speed that the player will move at.
	
	
	Vector3 movement;                   // The vector to store the direction of the player's movement.
	Animator anim;                      // Reference to the animator component.
	Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
	int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
	public float turnSmoothing = 15f;   // A smoothing value for turning the player.
	public bool reverseDirection;
	private float h,v;
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
		if(Network.isServer && Network.connections.Length == 0){
			return;
		}

		// allow user to move only own player
		if(networkView.isMine)
		{
			// Store the input axes. ( for PC and MAC )
			//h = Input.GetAxis("Horizontal");
			//v = Input.GetAxis("Vertical");

			// Store the input axes. ( for Android )
			h = Input.acceleration.x;
			v = Input.acceleration.y+0.6f;

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
			//Animating (h, v);
		}
	}
	
	
	void Move (float horizontal, float vertical)
	{
		// Set the movement vector based on the axis input.
		movement.Set (horizontal, -0.1f, vertical);
		
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

	// function used to customize synchronization of variables in a script 
	// watched by a network view
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		bool running = false;
		// owner of character writing the state of the character

		if(stream.isWriting)
		{
			// Create a boolean that is true if either of the input axes is non-zero.
			running = h != 0f || v != 0f;
			//Debug.Log("running = " + running);
			stream.Serialize(ref running);
			// Tell the animator whether or not the player is walking.
			anim.SetBool ("IsRunning", running);
		}
		// code run by non-owner of the player
		else
		{
			// fetch the running state of the player
			stream.Serialize(ref running);
			anim.SetBool ("IsRunning", running);
		}
		
	}
}
