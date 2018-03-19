﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour {


	/// <summary>
	/// Can not change in code
	/// </summary>
	public  int InitHealthPoint;

	[HideInInspector]
	public float healthPoint;



	[SerializeField]
	private Slider HpSlider;

	bool IsInShadow{get {return GetComponent<ShadowMapCaculate.ShadowCaculate>().isInShadow;}}

	[SerializeField]
	private float changeRate;


	private HealthState state;
	enum HealthState{
		ADDBLOOD,
		SUBBLOOD,
		NORMAL,
		WAIT,
		DEAD
	}
	// Use this for initialization
	void Start () {
		
		state=HealthState.NORMAL;
		healthPoint=InitHealthPoint;
	}
	
	// Update is called once per frame
	[SerializeField]
	int timeToRecovery;
	float time;
	void Update () {
		switch(state)
		{
			case HealthState.NORMAL:
				if(!IsInShadow)
				state=HealthState.SUBBLOOD;
				break;
			case HealthState.SUBBLOOD:
				if(IsInShadow){
					state=HealthState.WAIT;time=0;break;
				}
				else{
					 healthPoint=healthPoint- (changeRate*Time.deltaTime);
					
				}
				if(healthPoint<=0.01f)
				{state=HealthState.DEAD;
				GameOver();
				}
				break;


			case HealthState.ADDBLOOD:
				if(IsInShadow){
				if(healthPoint<InitHealthPoint) {healthPoint+=(changeRate*Time.deltaTime);}

				else  {healthPoint=InitHealthPoint; state=HealthState.NORMAL;}
				}

				else state=HealthState.SUBBLOOD;
				break;


			case HealthState.WAIT:
				time+=Time.deltaTime;
				if(time>timeToRecovery)
				{state=HealthState.ADDBLOOD;	break;}

				if(!IsInShadow){state=HealthState.SUBBLOOD;}
				break;
		}

	HpSlider.value=healthPoint/InitHealthPoint;
	}

   void GameOver()	
	{
      //UIController.SetMessageBoxContext("游戏结束");
	  UIController.RestartButton();
	  Time.timeScale=0;
		
	}

   
}
