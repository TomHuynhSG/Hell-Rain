/* Script to control the attacking behaviour of an enemy

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */

using UnityEngine;
using System.Collections;

public class MultiplayerEnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 1.5f;
    public int attackDamage = 10;

    Animator anim;
    GameObject[] players;
	GameObject player1, player2, attackedPlayer;
    MultiplayerPlayerHealth playerHealth;
	MultiplayerEnemyHealth enemyHealth;
	bool isAttacking;
    float timer;
	
    void Awake ()
    {
		// assuming there are 2 players
        players = GameObject.FindGameObjectsWithTag ("Player");
		player1 = players [0];
		player2 = players [1];
		enemyHealth = GetComponent<MultiplayerEnemyHealth>();
        anim = GetComponent <Animator> ();
		attackedPlayer = null;
		isAttacking = false;
    }

	// function called when an object is in the collider of the enemy game object
    void OnTriggerEnter (Collider other)
    {
		// check if any of the player is in the collider and attack if there is
        if(other.gameObject == player1 || other.gameObject == player2)
        {
			attackedPlayer = other.gameObject;
        }
    }

	// function called when an object leaves the collider of the enemy game object
    void OnTriggerExit (Collider other)
    {
		if(other.gameObject == player1 || other.gameObject == player2)
        {
			attackedPlayer = null;
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && attackedPlayer!=null && enemyHealth.currentHealth > 0)
        {
			Attack(attackedPlayer);
        }
    }

	// function to attack a player
    void Attack (GameObject player)
    {
		isAttacking = true;
		anim.SetTrigger("IsAttacking");
        timer = 0f;
		playerHealth = player.GetComponent <MultiplayerPlayerHealth> ();
        if(playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }
    }

	// function used to customize synchronization of variables in a script 
	// watched by a network view
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		// owner/creator of the game object ( always going to be the server)
		if(stream.isWriting)
		{
			stream.Serialize(ref isAttacking);
		}
		// code run by non-owner of the spider
		else
		{
			// fetch the attacking state of the spider
			stream.Serialize(ref isAttacking);
			anim.SetTrigger("IsAttacking");
		}
	}

}
