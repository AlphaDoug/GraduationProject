using UnityEngine;
using System.Collections;

public class ActivateChest : MonoBehaviour
{
    public delegate void OpenOneChest(int id, string des);
    public static event OpenOneChest OpenOneChestEvent;

    public Transform lid, lidOpen, lidClose;	// Lid, Lid open rotation, Lid close rotation
	public float openSpeed = 5F;				// Opening speed
	public bool canClose;						// Can the chest be closed
    public bool isAutoClose = true;                    // Is the chest auto close
	
	[HideInInspector]
	public bool _open;							// Is the chest opened

    private bool isClosed = true;
	void Update ()
    {
		if(_open)
        {
			ChestClicked(lidOpen.rotation);
		}
		else
        {
			ChestClicked(lidClose.rotation);
		}
        if (lid.rotation == lidClose.rotation)
        {
            isClosed = true;
        }
        else
        {
            isClosed = false;
        }
	}
	
	// Rotate the lid to the requested rotation
	void ChestClicked(Quaternion toRot)
    {
		if(lid.rotation != toRot)
        {
			lid.rotation = Quaternion.Lerp(lid.rotation, toRot, Time.deltaTime * openSpeed);
		}
        if (lid.rotation == lidOpen.rotation && isAutoClose)
        {
            _open = false;
        }
	}
	
	void OnMouseDown()
    {
        if (!isClosed)
        {
            return;
        }

        if (!_open)
        {
            Invoke("ShowResult", 1.0f);
        }

        if (canClose)
            _open = !_open;
        else
            _open = true;

	}

    private void ShowResult()
    {
        var id = GetComponent<Collection>().ID;
        var des = GetComponent<Collection>().DES;
        if (OpenOneChestEvent != null)
        {
            OpenOneChestEvent(id, des);
        }

    }
}
