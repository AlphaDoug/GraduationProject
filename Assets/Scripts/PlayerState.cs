using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player{
public class PlayerState  {

	protected PlayerController player;
	protected Transform transform;
	protected Rigidbody rigidbody;
	public PlayerState(PlayerController player)
	{
		this.player=player;
		transform=player.transform;
		rigidbody=player.GetComponent<Rigidbody>();
	}
	public virtual void Enter()
	{

	}
	public virtual void Update()
	{

	}
	public virtual void Exit()
	{

	}
	

}
}