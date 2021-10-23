/* Script to control the attacking behaviour of the player

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */

using UnityEngine;

public class MultiplayerEnemyManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject enemy;
	public GameObject spawnEffectFX;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;


    void Start ()
    {
		if (Network.isServer)
		{
        	InvokeRepeating ("Spawn", spawnTime, spawnTime);
		}
    }


    void Spawn ()
    {
        /*if(playerHealth.currentHealth <= 0f)
        {
            return;
        }*/

		// spawn enemies only if 1 other player is connected
		if(Network.connections.Length > 0)
		{
	        int spawnPointIndex = Random.Range (0, spawnPoints.Length);

	        Network.Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation,0);
			Network.Instantiate (spawnEffectFX, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation,0);
		}
    }
}
