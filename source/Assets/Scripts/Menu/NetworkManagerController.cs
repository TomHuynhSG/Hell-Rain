/* Script to setup network between two phones, and perform 
 * any operations related to the network 

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */

using UnityEngine;
using System.Collections;

public class NetworkManagerController : MonoBehaviour {
	private static string gameName = "IT project - Hell Rain";
	private bool refreshing;
	
	public Transform spawnObject1, spawnObject2, enemySpawnObject;
	public GameObject player, enemy;
	public GameObject playerPrefab, enemyPrefab;
	public Camera mainCamera;
	public MultiplayerCameraFollow camFollow;


	// array to keep information about hosts
	private HostData[] hostList;
	
	private float btnX;
	private float btnY;
	private float btnH;
	private float btnW;
	
	// Use this for initialization
	void Start () 
	{
		btnX = Screen.width * 0.05f;
		btnY = Screen.width * 0.05f;
		btnH = Screen.width * 0.1f;
		btnW = Screen.width * 0.1f;
	}

	/*void Start () 
	{
		GameObject button = GameObject.FindGameObjectWithTag ("connectButton");
		btnX = button.transform.position.x + 2.0f ;
		btnY = button.transform.position.x;
		btnH = button.transform.lossyScale.y;
		btnW = button.transform.lossyScale.x;
	}*/

	// function to spawn a player in the network
	public void spawnPlayer(Transform spawnObject)
	{
		player = (Network.Instantiate(playerPrefab, spawnObject.position, Quaternion.identity, 0)) as GameObject;

		// make sure player is not destroyed when loading another scene
		DontDestroyOnLoad (player);
		DontDestroyOnLoad (this);

	}

	// function to spawn an enemy in the network
	public void spawnEnemy(Transform spawnObject)
	{
		enemy = (Network.Instantiate(enemyPrefab, spawnObject.position, Quaternion.identity, 0)) as GameObject;
		DontDestroyOnLoad (enemy);
	}

	// function called when the button "Start Server" is clicked
	public void StartServer()
	{
		Network.InitializeServer (32, 25001, !(Network.HavePublicAddress()));
		MasterServer.RegisterHost (gameName, "Test Connection Game", "This is a game to test connection");
	}
	
	// function called when the button "Refresh Hosts" is clicked
	public void RefreshHostList()
	{
		MasterServer.RequestHostList (gameName);
		refreshing = true;
	}

	// function automatically called by Unity when a new scene is loaded
	void OnLevelWasLoaded(int level)
	{
		// multiplayer level was loaded
		if(level == 2){
			mainCamera = null;
			Network.isMessageQueueRunning = true;

			// get the camera of the scene and assign set its target to the player
			mainCamera = Camera.main;
			camFollow = mainCamera.GetComponent<MultiplayerCameraFollow>();
			/*if(mainCamera == null){
				Debug.Log("camera null");
			}
			else{
				Debug.Log("cam name : " + mainCamera.name);
			}*/
			camFollow.attachTarget (player.transform);
			/*if(camFollow.target == player.transform)
			{
				Debug.Log("set target successful");
			}*/
		}
	}

	// function automatically called by Unity
	// function is called by client when it connects to the server
	void OnConnectedToServer() 
	{
		spawnPlayer(spawnObject1);
		// stop receiving any messages(in order to avoid conflicts) and 
		// then load the new scene
		Network.isMessageQueueRunning = false;
		Application.LoadLevel (2);
	}
	
	// function automatically called by Unity when a server has been initialised
	void OnServerInitialized()
	{
		//Debug.Log ("Server initialised!");
		spawnPlayer (spawnObject2);
		// stop receiving any messages(in order to avoid conflicts) and 
		// then load the new scene
		Network.isMessageQueueRunning = false;
		Application.LoadLevel (2);
	}

	/* Debugging
	void OnMasterServerEvent(MasterServerEvent mse)
	{
		if (mse == MasterServerEvent.RegistrationSucceeded) {
			Debug.Log ("Registration successful");
			
		}
		
	}*/
	
	// Update is called once per frame by Unity
	void Update () 
	{
		if(refreshing)
		{
			// polling the host list might take some time so
			// we need to keep refreshing
			if(MasterServer.PollHostList().Length > 0)
			{
				refreshing = false;
				// debugging
				// Debug.Log (MasterServer.PollHostList().Length);
				hostList = MasterServer.PollHostList();
			}
		}
	}

	// function called automatically by Unity when a player disconnects
	void OnPlayerDisconnected(NetworkPlayer player) 
	{
		//Debug.Log("Clean up after player " + player);

		// remove any objects of that player from the network
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}

	// function automatically called by Unity
	void OnGUI(){
		if(!Network.isClient && !Network.isServer)
		{
			if(hostList != null)
			{
				for (int i=0; i<hostList.Length; i++) {
					if(GUI.Button(new Rect(btnX*1.5f+btnW,btnY*1.0f+(btnH*i),btnW*2,btnH*0.5f),
					              hostList[i].gameName))
					{
						Network.Connect(hostList[i]);
					}
				}
			}
		}
	}
}