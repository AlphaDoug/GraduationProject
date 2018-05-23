using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloText : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Invoke("SetDisable", 2.0f);
	}
	
    private void SetDisable()
    {
        gameObject.SetActive(false);
    }
}
