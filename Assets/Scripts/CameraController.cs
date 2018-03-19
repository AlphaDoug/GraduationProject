using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player{
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

	// Use this for initialization


		private PlayerController player{get{return transform.root. GetComponent<PlayerController>();}}


		[SerializeField]
	 	private float mininumVertical = -45f;
	  	[SerializeField]
        private float maxinumVertical =15f;//仰视和俯视的最高最低
        private float mininumVerticalReal;
        private float maxinumVerticalReal;


		 private float rotationVertical = 0f;

		 private float deltalY;

		

		private float distanceToPlayer;

		[SerializeField]
		private float normalDistance;
		

	void Start () {
			rotationVertical = transform.eulerAngles.x;
            mininumVerticalReal = transform.eulerAngles.x + mininumVertical;
            maxinumVerticalReal = transform.eulerAngles.x + maxinumVertical;
	}
	
	// Update is called once per frame
	void Update () {
			deltalY = Input.GetAxis("Mouse Y") * player.RotateSpeed * Time.deltaTime;

            //Debug.Log(deltalY);
            rotationVertical -= deltalY;
            rotationVertical = Mathf.Clamp(rotationVertical, mininumVerticalReal, maxinumVerticalReal);
            transform.eulerAngles = new Vector3(rotationVertical, transform.eulerAngles.y,transform.eulerAngles.z);
            float newYPos=  (rotationVertical-mininumVerticalReal)/(maxinumVerticalReal-mininumVertical)*(7.25f-2.5f)+2.5f;

        	transform.localPosition=new Vector3(transform.localPosition.x,newYPos,transform.localPosition.z);







			Vector3 dir=transform.position-player.transform.position;
		   	Ray ray =new Ray(player.transform.position,dir);
			RaycastHit hit;  
            if(Physics.Raycast(ray, out hit,  normalDistance,1<<0))  
            {  
					transform.position=Vector3.Lerp(transform.position,transform.position+(hit.point-transform.position)*0.8f,Time.deltaTime*10);
            }  
			else
				{
					transform.position=player.transform.position+dir.normalized*normalDistance;
				}
	}
}
}