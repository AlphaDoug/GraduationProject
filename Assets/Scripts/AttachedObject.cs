using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using LightControl;
using FallDown;
public class AttachedObject : MonoBehaviour {


	public Transform trans;
	public Vector3 targetPos;
	public Vector3 targetAngle;

	[SerializeField]
	private  List<GameObject> coliders;
	

	public MeshType selfType;
	[SerializeField]
	private DirectEnum triggerDir;

	[SerializeField]
	private int playerSize;


	public string dialogContex;

	// Use this for initialization
	void Start () {
		LightDirect.changeDirEvent+=ChangeTrigger;
		
		targetPos=trans.position;
		targetAngle=trans.eulerAngles;
	}

	void FallDown()
	{
			UIController.SetDialogBoxContext(dialogContex);
			
			foreach(Collider col in   GetComponentsInChildren<Collider>())
			col.enabled=false;
			GetComponent<FallDownEffect>().FallDownEvent();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void  ChangeTrigger(DirectEnum dir)
	{
		
		for(int i=0;i<coliders.Count;i++)
		{
			if(i==(int)dir)coliders[i].SetActive(true);
			else coliders[i].SetActive(false);
		}
	}


	public void TriggerEnter(Collider hit,DirectEnum dir)
    {
		PlayerAction.fallDown+=FallDown;
		if(dir!=triggerDir)return;
        if(hit.transform.root.tag != "Player")return;
        
          
		  UIController.SetMessageBoxContext("滑动滚轮调整大小，按G键附着");
            hit.transform.root.GetComponent<PlayerController>().canChangeSize = true;
            hit.transform.root.GetComponent<PlayerController>().TolerantPosY = hit.transform.position.y;
        
    }

    public void TriggerStay(Collider hit,DirectEnum dir)
    {
		if(dir!=triggerDir)return;
        if(hit.transform.root.tag != "Player")return;
        if(Input.GetKeyDown(KeyCode.G))
        {	
			if(Mathf.Abs( playerSize-hit.transform.root.GetComponent<PlayerController>().sizeStep)<2)
				if(hit.transform.root.GetComponent<PlayerController>().CurrentMesh==selfType)
            		StartCoroutine(hit.transform.root.GetComponent<PlayerAction>().ShadowAttachAction(targetPos, targetAngle));
        }


    }
    public void TriggerExit(Collider hit,DirectEnum dir)
    {
		if(dir!=triggerDir)return;
        if(hit.transform.root.tag != "Player")return;
        hit.transform.root.GetComponent<PlayerController>().canChangeSize = false;
			UIController.CloseMessageBox();
        PlayerAction.fallDown-=FallDown;
    }

    /// <summary>
	/// This function is called when the MonoBehaviour will be destroyed.
	/// </summary>
	void OnDestroy()
	{
		LightDirect.changeDirEvent-=ChangeTrigger;
		
	}
}
