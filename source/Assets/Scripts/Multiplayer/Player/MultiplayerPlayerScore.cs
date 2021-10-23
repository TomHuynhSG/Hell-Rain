/* Script to control the score of the player

 * Authors: Jason Lee, Erlangga Satria Gama, Minh Thong, Brian Kang */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MultiplayerPlayerScore : MonoBehaviour {
	public int score;

	// Use this for initialization
	void Start () {
		score = 0;
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		int scoreC = 0;
		//Sending score stat to server
		if(stream.isWriting)
		{
			scoreC = score;
			stream.Serialize(ref scoreC);
		}
		//Reading score stat to server
		else
		{
			stream.Serialize(ref scoreC);
			score = scoreC;
		}	
	}
}
