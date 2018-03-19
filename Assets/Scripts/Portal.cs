using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class Portal : MonoBehaviour
    {
        public GameObject otherPortal;
        public Image cameraImage;
        public float alphaChangedSpeed = 0.02f;

        public bool isCameraImageChanging = false;
        private bool isup;
        // Use this for initialization
        void Start()
        {

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
            m_player.transform.position = otherPortal.transform.position + new Vector3(0, 0, 0.15f);
            m_player.transform.LookAt(otherPortal.transform.position + new Vector3(0, 0, 0.4f));
        }

    }

}
