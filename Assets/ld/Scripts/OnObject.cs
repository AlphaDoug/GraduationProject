using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnObject : MonoBehaviour {

    public bool playerIsOn;

	void OnCollisionEnter(Collision collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            playerIsOn = true;
        }
    }

    void OnCollisionExit(Collision collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            playerIsOn = false;
        }
    }
}
