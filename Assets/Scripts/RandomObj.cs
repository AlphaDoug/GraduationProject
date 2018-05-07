using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObj : MonoBehaviour
{
    [SerializeField]
    private GameObject[] fangan;
	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < fangan.Length; i++)
        {
            fangan[i].SetActive(false);
        }
        int randomIndex = Random.Range(0, fangan.Length);
        fangan[randomIndex].SetActive(true);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
