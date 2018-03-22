using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecifiedLocationUI : MonoBehaviour
{
    public GameObject collectionUI;
    public GameObject specifiedLocationUI;
    public Text tipShowed;
    
	// Use this for initialization
	void Start ()
    {
        specifiedLocationUI.SetActive(true);
        collectionUI.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ShowTip(string str)
    {
        tipShowed.text = str;
    }
}
