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

    [Tooltip("Ѳ��������")]
    [SerializeField]
    private GameObject cruiseRadiusObj;

    [Tooltip("Ѳ���뾶")]
    [SerializeField]
    private float cruiseRadius;

    [Tooltip("�ȴ�ʱ��")]
    [SerializeField]
    private float waitTimeInterval;

    [Tooltip("�ƶ�ʱ��")]
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

        #region ����״̬����
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
    /// �����XZƽ����ѡ��һ����������
    /// </summary>
    private void RandomDirection()
    {
        angel = Random.Range(0, 360);
        direction = new Vector3(Mathf.Cos(angel), 0, Mathf.Sin(angel));
    }
    /// <summary>
    /// ÿ��һ��ʱ��ı�����״̬
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeState()
    {
        while (true)
        {
            #region �ȴ�״̬
            robotState = RobotState.wait;
            yield return new WaitForSeconds(waitTimeInterval);
            #endregion

            #region ϵͳѡ����
            RandomDirection();
            #endregion

            #region ��ת����ָ��λ��
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

            #region ֱ��
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
    /// ��ָ��������ֱ��
    /// </summary>
    private void MoveOnDirection()
    {
        h = 0;
        v = 1;
    }
    /// <summary>
    /// ��ת����ָ������
    /// </summary>
    /// <returns>��ת��ɷ�����,δ��ɷ��ؼ�</returns>
    private bool RotateToTarget()
    {
        if ((angel - transform.localEulerAngles.y) > 15 || (angel - transform.localEulerAngles.y) < -15)
        {
            if (direction.x > 0)//���������ڵ�һ ��������,��ʾ��Ҫ����ת
            {
                h = 1;
            }
            else if (direction.x < 0)//���������ڵڶ� ��������,��ʾ��Ҫ����ת
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
