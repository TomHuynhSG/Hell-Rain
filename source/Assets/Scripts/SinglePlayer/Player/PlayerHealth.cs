/* Script to control the health of the player

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
	//Player's starting health
    public int startingHealth = 100;
	//Player's current health
    public int currentHealth;
	//GUI Interface in Canvas showing player's remaining health
    public Slider healthSlider;
	//image shown when player is attacked
    public Image damageImage;
	//audio when player dies
    public AudioClip deathClip;
	//audio when player is attacked
	public AudioClip playerHurt;
	//flash animation properties
    public float flashSpeed = 3f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.3f);


    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
	PlayerAttacking playerAttacking;
    bool isDead;
    bool damaged;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
		playerAttacking = GetComponent <PlayerAttacking> ();
        currentHealth = startingHealth;
    }


    void Update ()
    {
		if(damaged){
			Invoke("DisplayDamageEffect",1f);
		}		
		else
		{
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
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
        damaged = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;
		playerAudio.clip = playerHurt;
        playerAudio.Play ();

        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        anim.SetTrigger ("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play ();

        playerMovement.enabled = false;
		playerAttacking.enabled = false;
    }
}
