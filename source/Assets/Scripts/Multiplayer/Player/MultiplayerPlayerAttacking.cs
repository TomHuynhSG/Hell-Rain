/* Script to control the attacking behaviour of the player

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */

using UnityEngine;
using System.Collections;

public class MultiplayerPlayerAttacking : MonoBehaviour {
	public static int damagePerAttack = 100;  // one hit one kill 
	public float timeBetweenAttacks = 3f;
	public float timeDuringAttack = 2f;


	Animator anim;
	bool IsAttacking;
	float timer;
	Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
	MultiplayerPlayerMovement playermovement;
	MultiplayerPlayerHealth playerhealth;
	int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
	public AudioClip AttackAudio;
	float camRayLength = 100f;          // The length of the ray from the camera into the scene.
	AudioSource playerAudio;
	//AudioSource attackingSound;

	// Use this for initialization
	void Awake () {
		anim = GetComponent <Animator> ();
		// Create a layer mask for the floor layer.
		floorMask = LayerMask.GetMask ("Floor");

		playerAudio = GetComponent <AudioSource> ();
		// Set up references.
		anim = GetComponent <Animator> ();

		playermovement = GetComponent<MultiplayerPlayerMovement> ();
		playerhealth = GetComponent<MultiplayerPlayerHealth> ();
		playerRigidbody = GetComponent <Rigidbody> ();
	
	}
	
	// Update is called once per frame
	void Update () {
		if(networkView.isMine && !playerhealth.isDead)
		{
			timer += Time.deltaTime;

			if(Input.GetButton ("Fire1") && timer >= timeBetweenAttacks)
			{
				TurningWithTouch();
				// make sure the player is attacking in both phones
				networkView.RPC("Attack",RPCMode.AllBuffered);
			}
		}
	
	}

	[RPC]
	void Attack ()
	{
		timer = 0f;

		IsAttacking=true;
		playermovement.enabled=false;
		playerAudio.clip = AttackAudio;
		playerAudio.Play ();
		anim.SetBool("IsAttacking",IsAttacking);
		Invoke("setAttackingToFalse",timeDuringAttack);
		Invoke("enableMovement",timeDuringAttack);
	
	}

	void setAttackingToFalse (){
		IsAttacking=false;
		anim.SetBool("IsAttacking",IsAttacking);
	}

	void enableMovement (){
		playermovement.enabled=true;
	}

	// function called when a game object is in the collider
	void OnTriggerEnter (Collider other)
	{
		// damage any enemy in the collider if the player is attacking
		if(networkView.isMine && other.gameObject.tag == "Enemy")
		{
			if (IsAttacking==true){
				MultiplayerEnemyHealth enemyHealth=other.GetComponent<MultiplayerEnemyHealth> ();
				enemyHealth.TakeDamage (gameObject, damagePerAttack);
			}
		}	
	}


	void TurningWithTouch ()
	{
		// Create a ray from the mouse cursor on screen in the direction of the camera.
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		// Create a RaycastHit variable to store information about what was hit by the ray.
		RaycastHit floorHit;
		
		// Perform the raycast and if it hits something on the floor layer...
		if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
		{
			// Create a vector from the player to the point on the floor the raycast from the mouse hit.
			Vector3 playerToMouse = floorHit.point - transform.position;
			
			// Ensure the vector is entirely along the floor plane.
			playerToMouse.y = 0f;
			
			// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			
			// Set the player's rotation to this new rotation.
			playerRigidbody.MoveRotation (newRotation);
		}  
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		
	}
}
