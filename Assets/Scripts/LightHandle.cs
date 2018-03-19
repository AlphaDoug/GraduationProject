using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightControl;
public class LightHandle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Q))
		{
			LightDirect.GetInstance().LastDirection();
		}
		if(Input.GetKeyDown(KeyCode.E))LightDirect.GetInstance().NextDirection();
	}
}
