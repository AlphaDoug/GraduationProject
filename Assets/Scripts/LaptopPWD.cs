using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaptopPWD : MonoBehaviour
{
    public GameObject nextPWD;
    [SerializeField]
    private string letter;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (nextPWD != null)
            {
                nextPWD.SetActive(true);
                gameObject.SetActive(false);
            }
            else
            {
                //最后一个密码被踩下
                Debug.Log("最后一个密码被踩下");
                gameObject.SetActive(false);
            }
            FindObjectOfType<ShowPassword>().AddLetter(letter);
        }
    }
}
