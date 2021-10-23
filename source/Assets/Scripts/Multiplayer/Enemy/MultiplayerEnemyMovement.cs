/* Script to control the health of an enemy

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */

using UnityEngine;
using System.Collections;

public class MultiplayerEnemyMovement : MonoBehaviour
{
	Transform player;
	MultiplayerPlayerHealth playerHealth;
	MultiplayerEnemyHealth enemyHealth;
	NavMeshAgent nav;
	
	
	void Awake ()
	{
		
		enemyHealth = GetComponent <MultiplayerEnemyHealth> ();
		nav = GetComponent <NavMeshAgent> ();
	}
	
	
	void Update ()
	{
		
		player = FindClosestPlayerAlive().transform;

		if(enemyHealth.currentHealth > 0 && player!=null)
		{
			nav.SetDestination (player.position);
		}
		else
		{
			nav.enabled = false;
		}
	}

	GameObject FindClosestPlayerAlive() {
		GameObject[] players;
		players = GameObject.FindGameObjectsWithTag("Player");
		GameObject closest=null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;

		foreach (GameObject player in players) {
			Vector3 diff = player.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			playerHealth = player.GetComponent <MultiplayerPlayerHealth> ();
			if (playerHealth.currentHealth>0 && curDistance < distance ) {
				closest = player;
				distance = curDistance;
			}
		}
		return closest;
	}
}
