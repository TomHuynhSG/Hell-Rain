/* Script to control the health of an enemy

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */

using UnityEngine;

public class MultiplayerEnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;

    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
	bool isDead, isSinking, isAttacked;

	GameObject killer;

	float destroy_timer;

    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        //hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;
		isAttacked = false;

		destroy_timer = 2.5f;
    }
	
    void Update ()
    {
		// enemy is already dead and is sinking
        if(isSinking)
        {
			// sink it under the ground for some time, and then remove it from the game
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
			destroy_timer -= Time.deltaTime;
			if(destroy_timer < 0){
				Network.RemoveRPCs(networkView.viewID);
				Network.Destroy (gameObject);
				//Destroy (gameObject, 2.5f);
			}
        }
    }

	// function used when the enemy is attacked
    public void TakeDamage (GameObject player, int amount)
    {
		// process only own objects
		// server is going to process everything and client just gets info from server
		if(isDead)
			return;

		enemyAudio.Play ();
		
		currentHealth -= amount;
		
		if(currentHealth <= 0)
		{
			// kill enemy in both client and server 
			networkView.RPC("Death",RPCMode.AllBuffered);
			killer = player;
		}
			
		// boolean so that other phone runs this code as well
		isAttacked = true;

    }

	[RPC]
	void Death ()
    {
        isDead = true;

        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }


    public void StartSinking ()
    {
        GetComponent <NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;
        MultiplayerPlayerScore killerScore = killer.GetComponent<MultiplayerPlayerScore>();
		killerScore.score += scoreValue;
    }

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		bool sinkBool = false, deadBool = false, attackedBool = false;
		int health = 0;
		// code run by owner of object
		if(stream.isWriting)
		{	
			health = currentHealth;
			sinkBool = isSinking;
			deadBool = isDead;
			attackedBool = isAttacked;

			stream.Serialize(ref health);
			stream.Serialize(ref sinkBool);
			stream.Serialize(ref deadBool);
			stream.Serialize(ref attackedBool);
			
		}
		// code run by non-owner of the spider, i.e client
		else
		{
			stream.Serialize(ref health);
			stream.Serialize(ref sinkBool);
			stream.Serialize(ref deadBool);
			stream.Serialize(ref attackedBool);
			currentHealth = health;
			isSinking = sinkBool;
			isDead = deadBool;
			//Debug.Log (currentHealth);

			if(attackedBool){
				//this.TakeDamage(MultiplayerPlayerAttacking.damagePerAttack);
				Death ();
			}
		}
		
	}
}
