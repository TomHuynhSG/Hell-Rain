/* Script to control the health of the player

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultiplayerPlayerHealth : MonoBehaviour
{
	//Player's starting health
    public static int startingHealth = 100;
	//Player's current health
    public int currentHealth;
	//GUI Interface in Canvas showing player's remaining health
    //public Slider healthSlider;
	//image shown when player is attacked
    public Image damageImage;
	//audio when player dies
    public AudioClip deathClip;
	//audio when player is attacked
	public AudioClip playerHurt;
	//flash animation properties
    public static float flashSpeed = 3f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.3f);

	public float sinkSpeed = 2.5f;
    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
	PlayerAttacking playerAttacking;
    public bool isDead;
    bool damaged;
	bool isSinking;

    void Awake ()
    {
		Debug.Log("Player Awake");
		isDead = false;
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
		playerAttacking = GetComponent <PlayerAttacking> ();
        currentHealth = startingHealth;
    }


    void Update ()
    {

		if(isSinking)
		{
			transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
		}
    }
    // change the screen color when player get damage
	public void DisplayDamageEffect(){
		damageImage.color = flashColour;
		damaged = false;
	}

	// function used to damage the player's health
    public void TakeDamage (int amount)
    {
		if(isDead || !networkView.isMine){
			return;
		}
        damaged = true;

        currentHealth -= amount;
		if(currentHealth <= 0 && !isDead)
		{
			//Debug.Log("Someone die");
			//Death ();
			// make sure the Death function is called by both phones
			networkView.RPC("Death",RPCMode.AllBuffered);
		}
		//Debug.Log("Someone hurt "+currentHealth);
        //healthSlider.value = currentHealth;
		playerAudio.clip = playerHurt;
        playerAudio.Play ();
    }

	[RPC]
    void Death ()
    {
        isDead = true;

        anim.SetTrigger ("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play ();

        playerMovement.enabled = false;
		playerAttacking.enabled = false;
    }

	public void StartSinking ()
	{
		GetComponent <Rigidbody> ().isKinematic = true;
		isSinking = true;
		if(Network.isServer){
			Network.RemoveRPCs(networkView.viewID);
			//Network.Destroy (gameObject);
			//Destroy (gameObject, 2.5f);
		}
	}

	// function used to customize synchronization of variables in a script 
	// watched by a network view
    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
		int healthC = 0;
		bool sinkBool = false;
        //Sending health stat to server
        if(stream.isWriting)
        {
			sinkBool = isSinking;
            healthC = currentHealth;
            stream.Serialize(ref healthC);
			stream.Serialize(ref sinkBool);
        }
        //Reading health stat to server
        else
        {
            stream.Serialize(ref healthC);
			stream.Serialize(ref sinkBool);
            currentHealth = healthC;
			isSinking = sinkBool;
        }
        
    }
}
