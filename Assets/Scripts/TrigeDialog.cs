using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigeDialog : MonoBehaviour {

	public string dialogContex;
	public string messageContex;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerEnter(Collider other)
	{
		UIController.SetMessageBoxContext(messageContex);
	}


	/// <summary>
	/// OnTriggerStay is called once per frame for every Collider other
	/// that is touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerStay(Collider other)
	{
		if(Input.GetKey(KeyCode.F))
		{
			UIController.SetDialogBoxContext(dialogContex);
			Player.PlayerController.CloseControll();
		}
	}



	/// <summary>
	/// OnTriggerExit is called when the Collider other has stopped touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerExit(Collider other)
	{
		UIController.CloseMessageBox();
	}
}
