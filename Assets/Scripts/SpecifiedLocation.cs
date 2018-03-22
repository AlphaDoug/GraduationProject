using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecifiedLocation : MonoBehaviour {

    public GameObject nextTriggerObj;
    public string locationTip;
	// Use this for initialization
	void Start ()
    {
        transform.parent.gameObject.GetComponent<SpecifiedLocationUI>().ShowTip("提示:" + locationTip);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (nextTriggerObj != null)
            {
                nextTriggerObj.SetActive(true);
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
                Debug.Log("到达最后一个指定地点!!!!");
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().SetNextGroupTrue();
            }
        }
    }
}
