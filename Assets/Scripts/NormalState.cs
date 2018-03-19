using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class NormalState : PlayerState
    {
        
        private float jumpTimmer;
        private RaycastHit rayHit;

        public Animator animator { get { return player.animator; } }


           private float rotationHorizontal = 0f;

   

        public NormalState(PlayerController player) : base(player)
        {

        }
        public override void Enter()
        {
            base.Enter();
            //上下移动的限制条件
           
        }
        public override void Update()
        {
            base.Update();
            Vector3 pos = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * player.MoveSpeed * Time.deltaTime;
            pos = transform.TransformDirection(pos);




            animator.SetFloat("Speed", pos.magnitude);


            if (Input.GetAxis("Vertical") != 0) animator.SetBool("IsStraight", true);

            else if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") != 0) animator.SetBool("IsStraight", false);




            if (pos.magnitude != 0 && player.sizeTransform.localScale != player.TolerantSize && player.canChangeSize)
            {
                player.GetComponent<PlayerAction>().RevertTolerantSize(player.TolerantSize, player.TolerantPosY);
            }
            rigidbody.MovePosition(transform.position + pos);
            transform.Rotate(0, Input.GetAxis("Mouse X") * player.RotateSpeed * Time.deltaTime, 0);
            transform.Rotate(-Input.GetAxis("Mouse Y") * player.RotateSpeed * Time.deltaTime, 0, 0);

            //控制摄像机上下的旋转  

            // Debug.Log(rotationVertical);
            //Jump();


        }
        public override void Exit()
        {
            base.Exit();
        }

        void Jump()
        {
            Debug.DrawRay(transform.position, Vector3.up * -player.DownRayLength, Color.red);
            if (Input.GetKey(KeyCode.Space))
            {
                //如果在改变了大小的情况下按空格则还原大小
                if (player.sizeTransform.localScale != player.TolerantSize && player.canChangeSize)
                {
                    player.GetComponent<PlayerAction>().RevertTolerantSize(player.TolerantSize, player.TolerantPosY);
                }

                if (Physics.Raycast(transform.position, Vector3.up * -1, out rayHit, player.DownRayLength))
                {
                    if (rayHit.transform.tag != "Player")
                    {
                        jumpTimmer += Time.deltaTime;
                    }
                    if (jumpTimmer >= player.JumpCriticalTime)
                    {
                        rigidbody.AddForce(new Vector3(0, player.HighJumpForce, 0));
                    }

                }
            }
            else if (jumpTimmer > 0)
            {
                if (jumpTimmer < player.JumpCriticalTime)
                {
                    rigidbody.AddForce(new Vector3(0, player.LowJumpForce, 0));
                }
                jumpTimmer = 0;
            }
        }



    }
}