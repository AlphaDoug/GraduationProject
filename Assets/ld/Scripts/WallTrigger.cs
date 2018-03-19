using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WallTrigger : MonoBehaviour {

    public GameObject obj;
	
	void OnTriggerEnter(Collider hit)
    {
        if(hit.tag == "Player")
        {
            if(!obj.GetComponent<OnObject>().playerIsOn)
            {
                hit.transform.DORotate(new Vector3(-90, 0, 0), 1);
            }
            else
            {
                hit.transform.DORotate(new Vector3(0, 180, 0), 1);
            }
            
        }

    }
}
