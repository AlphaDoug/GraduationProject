using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
namespace LightControl{
public enum DirectEnum
{
	 First,
	 Second,
	 Third
}

public class LightDirect : MonoBehaviour {

        // Use this for initialization
    
	public Transform lightObj;
	[SerializeField, SetProperty("Gradient")]
	private float gradient=21;
	public float Gradient
	{
		get{ return gradient;}
		set{gradient=value;lightObj.eulerAngles=new Vector3(gradient,lightObj.eulerAngles.y,lightObj.eulerAngles.z);}
	}
    [SerializeField, SetProperty("Direct")]
	DirectEnum direct;


	public static Action<DirectEnum> changeDirEvent;
	public DirectEnum Direct
	{
		get{return direct;}
		set{
			direct=value;
			Vector3 oldAngle=transform.eulerAngles;
			switch(value)
			{
				case DirectEnum.First:transform.eulerAngles=new Vector3(oldAngle.x,firstAngle,oldAngle.z);angleIndex=0; break;
				case DirectEnum.Second:transform.eulerAngles=new Vector3(oldAngle.x,secondAngle,oldAngle.z);angleIndex=1;break;
				case DirectEnum.Third:transform.eulerAngles=new Vector3(oldAngle.x,thirdAngle,oldAngle.z);angleIndex=2;break;
			}

			if(changeDirEvent!=null)changeDirEvent(value);
           // GetComponent<TriggerManager>().SetOnlyOneActive(value);//根据光照设置不同的影子触发器
		}
	}


	public float firstAngle;
	public float secondAngle;
	public float thirdAngle;

	int angleIndex=1;
	private static LightDirect instance;
	public static LightDirect GetInstance()
	{
		//if(instance!=null)
		return instance;
	}

	void Awake()
	{
		instance=this;
    
	}                                                                                                         
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	

	public void NextDirection()
	{

		
		angleIndex=(angleIndex+1)%3;
		if(angleIndex==0)angleIndex=1;
		Direct=(DirectEnum)angleIndex;
	}
	public void LastDirection()
	{
		angleIndex=(angleIndex+2)%3;
		if(angleIndex==0)angleIndex=2;
		Direct=(DirectEnum)angleIndex;
	}
}
}