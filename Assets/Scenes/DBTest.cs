using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBTest : MonoBehaviour
{
    private OOFormArray mForm = null;
    // Use this for initialization
    void Start ()
    {
        if (mForm == null)
        {
            mForm = OOFormArray.ReadFromResources("Data/Tables/Table_0");
        }

        Debug.Log(mForm.GetString("Name", 2));
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
