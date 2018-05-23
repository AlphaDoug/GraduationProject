using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCreator : MonoBehaviour
{
    public class RobotAttribute
    {
        public int RobotID;
        public float DirectionDampTime;
        public float WaitTimeInterval;
        public float MoveTimeInterval;
        public string PrefabPath;
        public string StartPosition
        {
            set
            {
                float x = float.Parse(value.Split(',')[0]);
                float y = float.Parse(value.Split(',')[1]);
                float z = float.Parse(value.Split(',')[2]);
                StartPositionVector3 = new Vector3(x, y, z);
            }
        }
        public Vector3 StartPositionVector3;
    }

    private OOFormArray robotForm = null;
    private List<RobotAttribute> robotAttributes = new List<RobotAttribute>();
    private void Awake()
    {
        if (robotForm == null)
        {
            robotForm = OOFormArray.ReadFromResources("Data/Tables/TbRobotConfig");
        }

        for (int i = 1; i < robotForm.mRowCount; i++)
        {
            var oneRobotAttribute = robotForm.GetObject<RobotAttribute>(i);
            robotAttributes.Add(oneRobotAttribute);
        }
    }

    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < robotAttributes.Count; i++)
        {
            if (!CreateRobot(robotAttributes[i]))
            {
                Debug.LogError("机器人未生成,请检查TbRobotConfig,ID为" + robotAttributes[i].RobotID);
            }         
        }

	}
	/// <summary>
    /// 创建一个机器人,成功返回真
    /// </summary>
    /// <param name="robotAttribute">机器人属性</param>
    /// <returns></returns>
    private bool CreateRobot(RobotAttribute robotAttribute)
    {
        if (robotAttribute == null)
        {
            return false;
        }
        try
        {
            GameObject robotRes = Resources.Load(robotAttribute.PrefabPath) as GameObject;
            GameObject robotObject = Instantiate(robotRes) as GameObject;
            robotObject.GetComponent<RobotAIController>().DirectionDampTime = robotAttribute.DirectionDampTime;
            robotObject.GetComponent<RobotAIController>().waitTimeInterval = robotAttribute.WaitTimeInterval;
            robotObject.GetComponent<RobotAIController>().moveTimeInterval = robotAttribute.MoveTimeInterval;
            robotObject.transform.position = robotAttribute.StartPositionVector3;
        }
        catch (System.Exception)
        {
            return false;
            throw;
        }
        return true;
    }

}
