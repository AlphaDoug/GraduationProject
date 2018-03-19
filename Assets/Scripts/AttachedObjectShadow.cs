using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using LightControl;
public class AttachedObjectShadow : MonoBehaviour {
	AttachedObject obj{get{return transform.parent.GetComponent<AttachedObject>();}}


	[SerializeField]
	private DirectEnum selfDir;
	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		switch(transform.name)
		{
			case "first" :	selfDir=DirectEnum.First;	break;
			case "second":	selfDir=DirectEnum.Second;	break;
			case "third" :	selfDir=DirectEnum.Third;	break;
			default : Debug.LogError("命名不规则");break;
		}
	}
	void OnTriggerEnter(Collider hit)
    {
		obj.TriggerEnter(hit,selfDir);
    }

    void OnTriggerStay(Collider hit)
    {
		obj.TriggerStay(hit,selfDir);
    }
    void OnTriggerExit(Collider hit)
    {
		obj.TriggerExit(hit,selfDir);
    }
	// Use this for initialization

}
