using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
namespace FallDown{
public class RemoteControllerEffect : FallDownEffect {

	public override void FallDownEvent()
	{
		 PlayerController player= ((PlayerController)FindObjectOfType(typeof( PlayerController)));
		player.GetComponent<LightHandle>().enabled=true;
	}
}
}