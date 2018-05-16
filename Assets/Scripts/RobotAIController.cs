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

    [Tooltip("巡航触发器")]
    [SerializeField]
    private GameObject cruiseRadiusObj;

    [Tooltip("巡航半径")]
    [SerializeField]
    private float cruiseRadius;

    [Tooltip("等待时间")]
    [SerializeField]
    private float waitTimeInterval;

    [Tooltip("移动时间")]
    [SerializeField]
    private float moveTimeInterval;

    private float h;
    private float v;
    private Collider cruiseRadiuscCollider;
    private bool isHaveCollider = false;
    private Vector3 direction;
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
        cruiseRadiuscCollider = cruiseRadiusObj.GetComponent<Collider>();
        cruiseRadiusObj.transform.localScale = new Vector3(cruiseRadius, 0.2f, cruiseRadius);

        StartCoroutine(ChangeState());
    }

    // Update is called once per frame
    void Update()
    {

        if (animator)
        {
            //h = Input.GetAxis("Horizontal");
            //v = Input.GetAxis("Vertical");

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
    
    public void OnTriggerEnterChild(Collider other)
    {
        isHaveCollider = true;
    }

    public void OnTriggerExitChild(Collider other)
    {
        isHaveCollider = false;
    }
    /// <summary>
    /// 随机在XZ平面上选择一个方向向量
    /// </summary>
    private void RandomDirection()
    {
        angel = Random.Range(0, 360);
        direction = new Vector3(Mathf.Cos(angel), 0, Mathf.Sin(angel));
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
                    continue;
                }
                yield return new WaitForEndOfFrame();
            }
            #endregion

            #region 直行
            moveStartTime = Time.time;
            robotState = RobotState.move;
            while (Time.time - moveStartTime < moveTimeInterval)
            {
                yield return new WaitForEndOfFrame();
                if (isHaveCollider)
                {
                    continue;
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
        if ((angel - transform.localEulerAngles.y) > 15 || (angel - transform.localEulerAngles.y) < -15)
        {
            if (direction.x > 0)//方向向量在第一 四象限中,表示需要先右转
            {
                h = 1;
            }
            else if (direction.x < 0)//方向向量在第二 三象限中,表示需要先左转
            {
                h = -1;
            }
            else
            {
                if (direction.z > 0)
                {
                    h = 0;
                    v = 1;
                }
                else
                {
                    h = -1;
                }
            }
            return false;
        }
        else
        {
            return true;
        }
    }


}
