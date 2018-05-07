using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class Portal : MonoBehaviour
    {
        public GameObject otherOutPosition;
        public GameObject thisOutPosition;
        public Image cameraImage;
        public float alphaChangedSpeed = 0.02f;

        public Vector3 outPositionVector3;
        private bool isCameraImageChanging = false;
        private bool isup;

        void Start()
        {
            if (thisOutPosition.transform.localPosition == Vector3.zero)
            {
                thisOutPosition.transform.localPosition = outPositionVector3;
            }
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            if (isCameraImageChanging)
            {
                if (cameraImage.color.a <= 0)
                {
                    isup = true;
                }
                if (cameraImage.color.a >= 1)
                {
                    isup = false;
                }
                if (isup)
                {
                    cameraImage.color = new Color(1, 1, 1, cameraImage.color.a + alphaChangedSpeed);
                }
                else
                {
                    cameraImage.color = new Color(1, 1, 1, cameraImage.color.a - alphaChangedSpeed);
                    if (cameraImage.color.a <= 0)
                    {
                        isCameraImageChanging = false;
                    }
                }
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("人物进入触发器");
                isCameraImageChanging = true;
                Invoke("ChangePosition", 1f);

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("人物离开触发器");
            }
        }

        private void ChangeViewColor()
        {

        }

        private void ChangePosition()
        {
            var m_player = GameObject.FindGameObjectWithTag("Player");
            var directionVector = otherOutPosition.transform.position - otherOutPosition.transform.parent.position;
            Debug.Log(otherOutPosition.transform.position);
            //m_player.transform.LookAt(otherOutPosition.transform);
            m_player.transform.position = otherOutPosition.transform.position;

        }

        public bool GetIsCameraImageChanging()
        {
            return isCameraImageChanging;
        }
    }

}
