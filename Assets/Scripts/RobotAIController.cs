using UnityEngine;
using System.Collections;

public class RobotAIController : MonoBehaviour
{
    public enum RobotState
    {
        wait = 1,
        rotate = 2,
        move = 3
    }

    private Animator animator;
    public float DirectionDampTime = .25f;

    public float waitTimeInterval;

    public float moveTimeInterval;

    public GameObject text;
    private float h;
    private float v;
    private Collider cruiseRadiuscCollider;
    private bool isHaveCollider = false;
    private float angel;
    private bool canMove = false;
    private float moveStartTime;
    private RobotState robotState;
    private bool isRotateCompleted = false;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator.layerCount >= 2)
            animator.SetLayerWeight(1, 1);
        StartCoroutine(ChangeState());
        StartCoroutine(ResetHitInfo());
    }

    // Update is called once per frame
    void Update()
    {

        if (animator)
        {
            animator.SetFloat("Speed", h * h + v * v);
            animator.SetFloat("Direction", h, DirectionDampTime, Time.deltaTime);
        }

        #region 自身状态更新
        switch (robotState)
        {
            case RobotState.wait:
                h = 0;
                v = 0;
                break;
            case RobotState.rotate:
                isRotateCompleted = RotateToTarget();
                break;
            case RobotState.move:
                MoveOnDirection();
                break;
            default:
                break;
        }
        #endregion

    }
    
    /// <summary>
    /// 随机在XZ平面上选择一个方向向量
    /// </summary>
    private void RandomDirection()
    {
        angel = Random.Range(0, 360);
    }
    /// <summary>
    /// 每隔一段时间改变自身状态
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeState()
    {
        while (true)
        {
            #region 等待状态
            robotState = RobotState.wait;
            yield return new WaitForSeconds(waitTimeInterval);
            #endregion

            #region 系统选择方向
            RandomDirection();
            #endregion

            #region 旋转自身到指定位置
            robotState = RobotState.rotate;
            while (!isRotateCompleted)
            {
                robotState = RobotState.rotate;
                if (isHaveCollider)
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
            isRotateCompleted = false;
            #endregion

            #region 直行
            moveStartTime = Time.time;
            robotState = RobotState.move;
            while (Time.time - moveStartTime < moveTimeInterval)
            {
                yield return new WaitForEndOfFrame();
                if (isHaveCollider)
                {
                    break;
                }
            }
            #endregion
 
        }
       
    }
    /// <summary>
    /// 在指定方向上直行
    /// </summary>
    private void MoveOnDirection()
    {
        h = 0;
        v = 1;
    }
    /// <summary>
    /// 旋转自身到指定方向
    /// </summary>
    /// <returns>旋转完成返回真,未完成返回假</returns>
    private bool RotateToTarget()
    {
        if ((angel - transform.localEulerAngles.y) > 8 || (angel - transform.localEulerAngles.y) < -8)
        {
            if (angel > 0 && angel <= 180)//方向向量在第一 四象限中,表示需要先右转
            {
                h = 1;
            }
            else//方向向量在第二 三象限中,表示需要先左转
            {
                h = -1;
            }
            return false;
        }   
        else
        {
            return true;
        }
    }
    /// <summary>
    /// 机器人和其他碰撞体产生碰撞
    /// </summary>
    /// <param name="hit"></param>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.name.Contains("Plane"))
        {
            return;
        }
        isHaveCollider = true;
    }

    IEnumerator ResetHitInfo()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            isHaveCollider = false;
        }
    }

    private void OnMouseDown()
    {
        GetComponent<AudioSource>().Play();
        robotState = RobotState.wait;
        text.SetActive(true);
    }
}
