using UnityEngine;
using System.Collections;

public class WaitingTextController : MonoBehaviour {
	GameObject WaitingText;

	// Use this for initialization
	void Start () {
		WaitingText = GameObject.FindGameObjectWithTag ("WaitingText");	
		//WaitingText = this.gameObject;
		//WaitingText.SetActive(false);
		//Debug.Log (WaitingText);
	}
	
	// Update is called once per frame
	void Update () {
		if(Network.isServer && Network.connections.Length == 0)
		{
			WaitingText.SetActive(true);
		}
		else
		{
			WaitingText.SetActive(false);
		}
	}
}
