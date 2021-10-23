/*using System;
using UnityEngine;
using System.Collections;
using NUnit.Framework;

public class MovementTest {
	GameObject playerObject= GameObject.FindGameObjectsWithTag("Player")[0]; 


	[Test]
	public void ExistenceOfPlayer(){
		Assert.That(playerObject!=null);
	}


	[Test]
	public void PlayerAboveTheGround(){
		Assert.That(playerObject.rigidbody.position.y >=-0.05000019);
	}

	[Test]
	public void PlayerUnderTheTemple(){
		Assert.That(playerObject.rigidbody.position.y <=2);
	}
	

	[Test]
	public void PlayerHeightConstant(){
		Assert.That((playerObject.rigidbody.position.y <=0.046)&&(playerObject.rigidbody.position.y >-0.051)   );
	}
	
}
*/